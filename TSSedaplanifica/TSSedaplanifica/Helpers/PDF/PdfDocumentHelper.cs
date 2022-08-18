using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Models.ApplicationUser;

namespace TSSedaplanifica.Helpers.PDF
{
    public class PdfDocumentHelper : IPdfDocumentHelper
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        private Document doc = new Document(PageSize.A4, 28f, 25f, 20f, 40f);
        private MemoryStream ms = new MemoryStream();
        private PdfWriter write;
        private BaseFont fontTitle = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
        private BaseFont fontTitleSub = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
        private iTextSharp.text.Font title35;
        private iTextSharp.text.Font titles10;
        private iTextSharp.text.Font titles12;
        private iTextSharp.text.Font titlesBlackBold12;
        private iTextSharp.text.Font paragraph;
        private iTextSharp.text.Font theader;
        private iTextSharp.text.Image logo;
        private PdfPTable myTable;

        public PdfDocumentHelper(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            write = PdfWriter.GetInstance(doc, ms);
            title35 = new iTextSharp.text.Font(fontTitle, 35f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0));
            titles10 = new iTextSharp.text.Font(fontTitleSub, 10f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0));
            titles12 = new iTextSharp.text.Font(fontTitleSub, 12f, iTextSharp.text.Font.BOLD, new BaseColor(0, 127, 0));
            titlesBlackBold12 = new iTextSharp.text.Font(fontTitle, 12f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0));
            paragraph = new iTextSharp.text.Font(fontTitleSub, 9f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
            theader = new iTextSharp.text.Font(fontTitle, 12f, iTextSharp.text.Font.BOLD, new BaseColor(140, 40, 74));
            logo = iTextSharp.text.Image.GetInstance(Path.Combine(_env.WebRootPath, "images", "LogoTotSp.png"));
            doc.AddAuthor("SedaPlanifica");

            float proporcion = logo.Width / logo.Height;
            logo.ScaleAbsoluteWidth(150);
            logo.ScaleAbsoluteHeight(40 * proporcion);
            logo.SetAbsolutePosition(0, 567f);

            myTable = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100f };

            myTable.AddCell(new PdfPCell(logo) { Border = 0, Rowspan = 4, VerticalAlignment = Element.ALIGN_MIDDLE });
            myTable.AddCell(new PdfPCell(new Phrase("Gobernación de Arauca", paragraph)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            myTable.AddCell(new PdfPCell(new Phrase("Secretaría Departamental de Educación", paragraph)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            myTable.AddCell(new PdfPCell(new Phrase("Calle 20 con carrera 21", paragraph)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            myTable.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MMM/yyyy"), paragraph)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

        }

        public async Task<MemoryStream> ReportListAsync(string title)
        {

            doc.AddTitle(title);
            write.PageEvent = new PageEventHelper();

            doc.Open();

            doc.Add(myTable);

            doc.Add(new Phrase(" "));


            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };
            myTable.AddCell(new PdfPCell(new Phrase($"Listado {title.ToLower()}", titles12)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            doc.Add(myTable);
            doc.Add(new Phrase("\n"));


            if (title== "Categorías")
            {
                await CategoryEntity();
            }
            else if (title== "Clases de categorías")
            {
                await ClasesOfCategoriesEntity();
            }
            else if (title== "Unidades de medida")
            {
                await MeasureUnitEntity();
            }
            else if (title== "Estados de solicitud")
            {
                await SolicitStateEntity();
            }
            else if (title== "Elementos")
            {
                await ProductEntity();
            }
            else if (title== "Usuarios")
            {
                await ApplicationUserEntity();
            }
            else if (title== "Schools")
            {
                await SchoolsEntity();
            }

            doc.Add(myTable);

            doc.Close();

            write.Close();

            return ms;
        }

        public async Task<MemoryStream> ReportSoliAsync(int id)
        {
            doc.AddTitle("Solicitud");
            write.PageEvent = new PageEventHelper();

            doc.Open();

            doc.Add(myTable);

            doc.Add(new Phrase(" "));

            Solicit model = await _context.Solicits
                                                .Include(s=>s.School)
                                                .Include(s=>s.SolicitStates)
                                                .Include(s=>s.SolicitDetails).ThenInclude(d=>d.Product)
                                                .Where(s=>s.Id==id)
                                                .FirstOrDefaultAsync();


            myTable = new PdfPTable(new float[] { 30f,70F }) { WidthPercentage = 100f };

            PdfPCell pdfPCell1 = new PdfPCell(new Phrase($"Institución educativa:", titlesBlackBold12)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 5f, PaddingBottom = 5f };
            PdfPCell pdfPCell2 = new PdfPCell(new Phrase(model.School.Name, theader)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 5f, PaddingBottom = 5f };
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);

            pdfPCell1.Phrase = new Phrase("Descripción          :", titlesBlackBold12);
            pdfPCell1.Border = 0;
            pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell1.PaddingTop = 5f;
            pdfPCell1.PaddingBottom = 5f;

            pdfPCell2.Phrase = new Phrase(model.Description, theader);
            pdfPCell2.Border = 0;
            pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell2.PaddingTop = 5f;
            pdfPCell2.PaddingBottom = 5f;
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);

            pdfPCell1.Phrase = new Phrase("Fehca                :", titlesBlackBold12);
            pdfPCell1.Border = 0;
            pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell1.PaddingTop = 5f;
            pdfPCell1.PaddingBottom = 5f;

            pdfPCell2.Phrase = new Phrase(model.DateOfSolicit.ToString(), theader);
            pdfPCell2.Border = 0;
            pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell2.PaddingTop = 5f;
            pdfPCell2.PaddingBottom = 5f;
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);

            pdfPCell1.Phrase = new Phrase("Estado               :", titlesBlackBold12);
            pdfPCell1.Border = 0;
            pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell1.PaddingTop = 5f;
            pdfPCell1.PaddingBottom = 5f;

            pdfPCell2.Phrase = new Phrase(model.SolicitStates.Name, theader);
            pdfPCell2.Border = 0;
            pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell2.PaddingTop = 5f;
            pdfPCell2.PaddingBottom = 5f;
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);

            doc.Add(myTable);
            doc.Add(new Phrase("\n"));

            myTable = new PdfPTable(new float[] { 60f, 20f,20f }) { WidthPercentage = 100f };

            PdfPCell pdfPCell3 = new PdfPCell(new Phrase("Elemento", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell4 = new PdfPCell(new Phrase("Cantidad", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell5 = new PdfPCell(new Phrase("Aprobado", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCell3);
            myTable.AddCell(pdfPCell4);
            myTable.AddCell(pdfPCell5);

            int cont = 1;

            foreach (var item in model.SolicitDetails)
            {

                pdfPCell3.Phrase = new Phrase(item.Product.FullName, paragraph);
                pdfPCell3.Border = 0;

                pdfPCell4.Phrase = new Phrase(item.Quantity.ToString(), paragraph);
                pdfPCell4.Border = 0;

                pdfPCell5.Phrase = new Phrase(item.DeliveredQuantity.ToString(), paragraph);
                pdfPCell5.Border = 0;

                if (cont % 2 == 0)
                {
                    pdfPCell3.BackgroundColor = BaseColor.WHITE;
                    pdfPCell4.BackgroundColor = BaseColor.WHITE;
                    pdfPCell5.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCell3.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell4.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell5.BackgroundColor = new BaseColor(248, 248, 248);
                }

                myTable.AddCell(pdfPCell3);
                myTable.AddCell(pdfPCell4);
                myTable.AddCell(pdfPCell5);

                cont++;
            }

            doc.Add(myTable);

            doc.Close();

            write.Close();

            return ms;
        }
        
        public async Task<MemoryStream> ReportSchoolAsync(int id)
        {
            doc.AddTitle("Institución educativa");

            write.PageEvent = new PageEventHelper();

            doc.Open();

            doc.Add(myTable);

            doc.Add(new Phrase(" "));

            School model = await _context.Schools
                                        .Include(s => s.SchoolCampus)
                                        .Include(c=>c.City)
                                        .Include(z=>z.Zone)
                                        .Include(u=>u.SchoolUsers).ThenInclude(u=>u.ApplicationUser)
                                        .Where(s => s.Id == id)
                                        .FirstOrDefaultAsync();

            myTable = new PdfPTable(new float[] { 30f, 70F }) { WidthPercentage = 100f };

            PdfPCell pdfPCell1 = new PdfPCell(new Phrase($"Institución educativa:", titlesBlackBold12)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 5f, PaddingBottom = 5f };
            PdfPCell pdfPCell2 = new PdfPCell(new Phrase(model.Name, theader)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 5f, PaddingBottom = 5f };
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);

            pdfPCell1.Phrase = new Phrase("Código Dane          :", titlesBlackBold12);
            pdfPCell1.Border = 0;
            pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell1.PaddingTop = 5f;
            pdfPCell1.PaddingBottom = 5f;

            pdfPCell2.Phrase = new Phrase(model.DaneCode, theader);
            pdfPCell2.Border = 0;
            pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell2.PaddingTop = 5f;
            pdfPCell2.PaddingBottom = 5f;
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);
            
            pdfPCell1.Phrase = new Phrase("Rector               :", titlesBlackBold12);
            pdfPCell1.Border = 0;
            pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell1.PaddingTop = 5f;
            pdfPCell1.PaddingBottom = 5f;
            string Username = model.SchoolUsers.Count() == 0 ? "" : model.SchoolUsers.FirstOrDefault().ApplicationUser.FullName;
            pdfPCell2.Phrase = new Phrase(Username, theader);
            pdfPCell2.Border = 0;
            pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell2.PaddingTop = 5f;
            pdfPCell2.PaddingBottom = 5f;
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);

            pdfPCell1.Phrase = new Phrase("Dirección            :", titlesBlackBold12);
            pdfPCell1.Border = 0;
            pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell1.PaddingTop = 5f;
            pdfPCell1.PaddingBottom = 5f;

            pdfPCell2.Phrase = new Phrase(model.Address, theader);
            pdfPCell2.Border = 0;
            pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell2.PaddingTop = 5f;
            pdfPCell2.PaddingBottom = 5f;
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);
            
            pdfPCell1.Phrase = new Phrase("Municipio            :", titlesBlackBold12);
            pdfPCell1.Border = 0;
            pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell1.PaddingTop = 5f;
            pdfPCell1.PaddingBottom = 5f;

            pdfPCell2.Phrase = new Phrase(model.City.Name, theader);
            pdfPCell2.Border = 0;
            pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell2.PaddingTop = 5f;
            pdfPCell2.PaddingBottom = 5f;
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);

            pdfPCell1.Phrase = new Phrase("Zona                 :", titlesBlackBold12);
            pdfPCell1.Border = 0;
            pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell1.PaddingTop = 5f;
            pdfPCell1.PaddingBottom = 5f;

            pdfPCell2.Phrase = new Phrase(model.Zone.Name, theader);
            pdfPCell2.Border = 0;
            pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell2.PaddingTop = 5f;
            pdfPCell2.PaddingBottom = 5f;
            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);


            doc.Add(myTable);
            doc.Add(new Phrase("\n"));

            myTable = new PdfPTable(new float[] { 40f, 20f, 20f, 20F }) { WidthPercentage = 100f };

            PdfPCell pdfPCell3 = new PdfPCell(new Phrase("Sede", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell4 = new PdfPCell(new Phrase("Código Dane", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell5 = new PdfPCell(new Phrase("Coordinador", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell6 = new PdfPCell(new Phrase("Dirección", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_CENTER, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCell3);
            myTable.AddCell(pdfPCell4);
            myTable.AddCell(pdfPCell5);
            myTable.AddCell(pdfPCell6);

            List<School> schols = await _context.Schools
                                        .Include(s => s.SchoolCampus)
                                        .Include(c => c.City)
                                        .Include(z => z.Zone)
                                        .Include(u => u.SchoolUsers).ThenInclude(u => u.ApplicationUser)
                                        .Where(s => s.SchoolCampus.Id == id)
                                        .OrderBy(s=>s.Name)
                                        .ToListAsync();

            int cont = 1;

            foreach (var item in schols)
            {

                pdfPCell3.Phrase = new Phrase(item.Name, paragraph);
                pdfPCell3.Border = 0;

                pdfPCell4.Phrase = new Phrase(item.DaneCode, paragraph);
                pdfPCell4.Border = 0;
                Username = item.SchoolUsers.Count == 0 ? "" : item.SchoolUsers.FirstOrDefault().ApplicationUser.FullName;
                pdfPCell5.Phrase = new Phrase(Username, paragraph);
                pdfPCell5.Border = 0;

                pdfPCell6.Phrase = new Phrase(item.Address, paragraph);
                pdfPCell6.Border = 0;

                if (cont % 2 == 0)
                {
                    pdfPCell3.BackgroundColor = BaseColor.WHITE;
                    pdfPCell4.BackgroundColor = BaseColor.WHITE;
                    pdfPCell5.BackgroundColor = BaseColor.WHITE;
                    pdfPCell6.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCell3.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell4.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell5.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell6.BackgroundColor = new BaseColor(248, 248, 248);
                }

                myTable.AddCell(pdfPCell3);
                myTable.AddCell(pdfPCell4);
                myTable.AddCell(pdfPCell5);
                myTable.AddCell(pdfPCell6);

                cont++;
            }



            doc.Add(myTable);

            doc.Close();

            write.Close();

            return ms;
        }

        private async Task SchoolsEntity()
        {
            myTable = new PdfPTable(new float[] { 31f, 11f, 17f, 12f, 10f }) { WidthPercentage = 100f };

            PdfPCell pdfPCell1 = new PdfPCell(new Phrase("Descripción", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell2 = new PdfPCell(new Phrase("Código dane", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell3 = new PdfPCell(new Phrase("Dirección", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell4 = new PdfPCell(new Phrase("Municipio", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCell5 = new PdfPCell(new Phrase("Zona", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCell1);
            myTable.AddCell(pdfPCell2);
            myTable.AddCell(pdfPCell3);
            myTable.AddCell(pdfPCell4);
            myTable.AddCell(pdfPCell5);

            doc.Add(myTable);

            myTable = new PdfPTable(new float[] { 31f, 11f, 17f, 12f, 10f }) { WidthPercentage = 100f };

            List<School> model = await _context.Schools
                                                .Include(s => s.City)
                                                .Include(s => s.Zone)
                                                .Where(s => s.SchoolCampus == null)
                                                .ToListAsync();

            int cont = 1;
            foreach (var item in model)
            {

                pdfPCell1.Phrase = new Phrase(item.Name, paragraph);
                pdfPCell1.Border = 0;
                pdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfPCell2.Phrase = new Phrase(item.DaneCode, paragraph);
                pdfPCell2.Border = 0;
                pdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfPCell3.Phrase = new Phrase(item.Address, paragraph);
                pdfPCell3.Border = 0;
                pdfPCell3.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfPCell4.Phrase = new Phrase(item.City.Name, paragraph);
                pdfPCell4.Border = 0;
                pdfPCell4.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfPCell5.Phrase = new Phrase(item.Zone.Name, paragraph);
                pdfPCell5.Border = 0;
                pdfPCell5.HorizontalAlignment = Element.ALIGN_LEFT;


                if (cont % 2 == 0)
                {
                    pdfPCell1.BackgroundColor = BaseColor.WHITE;
                    pdfPCell2.BackgroundColor = BaseColor.WHITE;
                    pdfPCell3.BackgroundColor = BaseColor.WHITE;
                    pdfPCell4.BackgroundColor = BaseColor.WHITE;
                    pdfPCell5.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCell1.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell2.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell3.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell4.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCell5.BackgroundColor = new BaseColor(248, 248, 248);
                }

                myTable.AddCell(pdfPCell1);
                myTable.AddCell(pdfPCell2);
                myTable.AddCell(pdfPCell3);
                myTable.AddCell(pdfPCell4);
                myTable.AddCell(pdfPCell5);

                cont++;
            }
        }

        private async Task ApplicationUserEntity()
        {
            myTable = new PdfPTable(new float[] { 50f, 30f, 20f }) { WidthPercentage = 100f };

            PdfPCell pdfPCellName = new PdfPCell(new Phrase("Usuario", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCellEmail = new PdfPCell(new Phrase("Correro electrónico", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCellRol = new PdfPCell(new Phrase("Rol", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCellName);
            myTable.AddCell(pdfPCellEmail);
            myTable.AddCell(pdfPCellRol);

            doc.Add(myTable);

            myTable = new PdfPTable(new float[] { 50f, 30f, 20f }) { WidthPercentage = 100f };

            var user = await (from U in _context.Users
                              join E in _context.UserRoles on U.Id equals E.UserId into userRroleRela
                              from UR in userRroleRela.DefaultIfEmpty()
                              select new { Id = U.Id, FullName = U.FullName, Name = UR.RoleId ?? string.Empty, U.Email }).ToListAsync();

            List<RoleUserModelView> model = user.Select(u => new RoleUserModelView()
            {
                UserId = u.Id,
                FullName = u.FullName,
                RoleId = u.Name == "" ? String.Empty : _context.Roles.Where(r => r.Id == u.Name).FirstOrDefault().Name,
                email = u.Email
            }).OrderBy(u=>u.FullName).ToList();

            int cont = 1;

            foreach (var item in model)
            {

                pdfPCellName.Phrase = new Phrase(item.FullName, paragraph);
                pdfPCellName.Border = 0;
                pdfPCellName.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfPCellEmail.Phrase = new Phrase(item.email, paragraph);
                pdfPCellEmail.Border = 0;
                pdfPCellEmail.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfPCellRol.Phrase = new Phrase(item.RoleId, paragraph);
                pdfPCellRol.Border = 0;
                pdfPCellRol.HorizontalAlignment = Element.ALIGN_LEFT;


                if (cont % 2 == 0)
                {
                    pdfPCellName.BackgroundColor = BaseColor.WHITE;
                    pdfPCellEmail.BackgroundColor = BaseColor.WHITE;
                    pdfPCellRol.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCellName.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCellEmail.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCellRol.BackgroundColor = new BaseColor(248, 248, 248);
                }

                myTable.AddCell(pdfPCellName);
                myTable.AddCell(pdfPCellEmail);
                myTable.AddCell(pdfPCellRol);

                cont++;
            }
        }

        private async Task ProductEntity()
        {
            myTable = new PdfPTable(new float[] { 70f, 10f, 20f }) { WidthPercentage = 100f };

            PdfPCell pdfPCellName = new PdfPCell(new Phrase("Elemento", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCellMeasure = new PdfPCell(new Phrase("Unidad", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };
            PdfPCell pdfPCellClass = new PdfPCell(new Phrase("#Categorías", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCellName);
            myTable.AddCell(pdfPCellMeasure);
            myTable.AddCell(pdfPCellClass);

            doc.Add(myTable);

            myTable = new PdfPTable(new float[] { 70f,10f,20f }) { WidthPercentage = 100f };

            List<Product> model = await _context.Products
                                                .Include(p=>p.MeasureUnit)
                                                .Include(p=>p.ProductCategories)
                                                .ToListAsync();

            int cont = 1;

            foreach (var item in model)
            {

                pdfPCellName.Phrase = new Phrase(item.FullName, paragraph);
                pdfPCellName.Border = 0;
                pdfPCellName.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfPCellMeasure.Phrase = new Phrase(item.MeasureUnit.Name, paragraph);
                pdfPCellMeasure.Border = 0;
                pdfPCellMeasure.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfPCellClass.Phrase = new Phrase(item.CategoriesNumber.ToString(), paragraph);
                pdfPCellClass.Border = 0;
                pdfPCellClass.HorizontalAlignment = Element.ALIGN_CENTER;


                if (cont % 2 == 0)
                {
                    pdfPCellName.BackgroundColor = BaseColor.WHITE;
                    pdfPCellMeasure.BackgroundColor = BaseColor.WHITE;
                    pdfPCellClass.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCellName.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCellMeasure.BackgroundColor = new BaseColor(248, 248, 248);
                    pdfPCellClass.BackgroundColor = new BaseColor(248, 248, 248);
                }

                myTable.AddCell(pdfPCellName);
                myTable.AddCell(pdfPCellMeasure);
                myTable.AddCell(pdfPCellClass);

                cont++;
            }
        }

        private async Task SolicitStateEntity()
        {
            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            PdfPCell pdfPCell = new PdfPCell(new Phrase("Descripción", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCell);
            doc.Add(myTable);

            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            List<SolicitState> model = await _context.SolicitStates.ToListAsync();

            int cont = 1;

            foreach (var item in model)
            {

                pdfPCell.Phrase = new Phrase(item.Name, paragraph);
                pdfPCell.Border = 0;
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                if (cont % 2 == 0)
                {
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCell.BackgroundColor = new BaseColor(248, 248, 248);
                }

                myTable.AddCell(pdfPCell);

                cont++;
            }
        }

        private async Task MeasureUnitEntity()
        {
            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            PdfPCell pdfPCell = new PdfPCell(new Phrase("Descripción", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCell);
            doc.Add(myTable);

            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            List<MeasureUnit> model = await _context.MeasureUnits.ToListAsync();

            int cont = 1;

            foreach (var item in model)
            {

                pdfPCell.Phrase = new Phrase(item.Name, paragraph);
                pdfPCell.Border = 0;
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                if (cont % 2 == 0)
                {
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCell.BackgroundColor = new BaseColor(248, 248, 248);
                }

                myTable.AddCell(pdfPCell);

                cont++;
            }
        }

        private async Task CategoryEntity()
        {
            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            PdfPCell pdfPCell = new PdfPCell(new Phrase("Descripción", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCell);
            doc.Add(myTable);

            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            List<Category> model = await _context.Categories.ToListAsync();
            int cont = 1;
            foreach (var item in model)
            {

                pdfPCell.Phrase = new Phrase(item.Name, paragraph);
                pdfPCell.Border = 0;
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                if (cont % 2 == 0)
                {
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCell.BackgroundColor = new BaseColor(248, 248, 248);
                }

                myTable.AddCell(pdfPCell);

                cont++;
            }
        }

        private async Task ClasesOfCategoriesEntity()
        {
            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            PdfPCell pdfPCell = new PdfPCell(new Phrase("Descripción", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };

            myTable.AddCell(pdfPCell);
            doc.Add(myTable);

            myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            List<CategoryType> model = await _context.CategoryTypes.ToListAsync();
            int cont = 1;
            foreach (var item in model)
            {

                pdfPCell.Phrase = new Phrase(item.Name, paragraph);
                pdfPCell.Border = 0;
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                if (cont % 2 == 0)
                {
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                }
                else
                {
                    pdfPCell.BackgroundColor = new BaseColor(248, 248, 248);
                }

                //myTable.AddCell(new PdfPCell(new Phrase(item.Name, paragraph)) { Border = 0, BorderWidthTop = 1.99f, BorderColorTop = new BaseColor(195, 195, 195), PaddingTop = 10f, HorizontalAlignment = Element.ALIGN_LEFT });
                //myTable.AddCell(new PdfPCell(new Phrase(item.Name, paragraph)) { Border = 0, BorderWidthTop = 1.99f, BorderColorTop = new BaseColor(44, 117, 185), PaddingTop = 10f, HorizontalAlignment = Element.ALIGN_LEFT });

                myTable.AddCell(pdfPCell);

                cont++;
            }
        }

    }
}
