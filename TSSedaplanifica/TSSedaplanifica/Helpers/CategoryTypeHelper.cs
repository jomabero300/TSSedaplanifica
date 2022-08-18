using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Common;
using TSSedaplanifica.Data;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Helpers
{
    public class CategoryTypeHelper : ICategoryTypeHelper
    {
        private readonly ApplicationDbContext _context;

        private const string _name = "Clase de categoría";

        public CategoryTypeHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddUpdateAsync(CategoryType model)
        {
            Response response = new Response() { IsSuccess = true };

            if (model.Id == 0)
            {
                _context.CategoryTypes.Add(model);

                response.Message = $"{_name} guardado satisfactoriamente.!!!";
            }
            else
            {
                _context.CategoryTypes.Update(model);

                response.Message = $"{_name} actualizado satisfactoriamente.!!!";
            }


            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplica"))
                {
                    response.Message = $"Ya existe una {_name} con el mismo nombre.";
                }
                else
                {
                    response.Message = dbUpdateException.InnerException.Message;
                }

                response.IsSuccess = false;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;

                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<CategoryType> ByIdAsync(int id)
        {
            CategoryType model = await _context.CategoryTypes.FindAsync(id);

            return model;
        }

        public async Task<List<CategoryType>> ComboAsync()
        {
            List<CategoryType> model = await _context.CategoryTypes.ToListAsync();

            model.Add(new CategoryType { Id = 0, Name = "[Seleccione una clase de categoría..]" });

            return model.OrderBy(m => m.Name).ToList();
        }
        public async Task<List<CategoryType>> ComboAsync(int categoryid)
        {
            //List<CategoryType> model = await _context.CategoryTypeDers
            //                                            .Include(c=>c.CategoryType)
            //                                            .Where(c=>c.Category.Id== categoryid)
            //                                            .Select(s=>s.CategoryType)
            //                                            .ToListAsync();

            List<CategoryTypeDer> ctd = await _context.CategoryTypeDers
                                                        .Include(c => c.Category)
                                                        .Include(s=>s.CategoryType)
                                                        .Where(c => c.Category.Id == categoryid)
                                                        .ToListAsync();

            List<CategoryType> ct = new List<CategoryType>();

            foreach (var c in ctd)
            {
                ct.Add(new CategoryType { Id = c.CategoryType.Id, Name = c.CategoryType.Name });
            }

            List<CategoryType> model = await _context.CategoryTypes.Where(c => !ct.Contains(c)).ToListAsync();

            model.Add(new CategoryType { Id = 0, Name = "[Seleccione una clase de categoría..]" });

            return model.OrderBy(m => m.Name).ToList();
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response response = new Response() { IsSuccess = true };

            CategoryType model = await _context.CategoryTypes.FindAsync(id);

            try
            {
                response.Message = $"{_name} borrado(a) satisfactoriamente!!!";

                _context.CategoryTypes.Remove(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                if (ex.Message.Contains("REFERENCE"))
                {
                    response.Message = $"{_name} no se puede eliminar porque tiene registros relacionados";
                }
                else
                {
                    response.Message = ex.Message;
                }

            }

            return response;
        }

        public async Task<List<CategoryType>> ListAsync()
        {
            List<CategoryType> model = await _context.CategoryTypes.ToListAsync();

            return model.OrderBy(m => m.Name).ToList();
        }

        //public async Task<MemoryStream> ReportAsync()
        //{
            //Document doc = new Document(PageSize.A4, 28f, 25f, 20f, 20f);
            //MemoryStream ms = new MemoryStream();
            //PdfWriter write = PdfWriter.GetInstance(doc, ms);
            //doc.AddAuthor("SedaPlanifica");
            //doc.AddTitle("Clases de categorías");
            //doc.Open();

            //BaseFont fontTitle = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
            //BaseFont fontTitleSub = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);

            //iTextSharp.text.Font title35 = new iTextSharp.text.Font(fontTitle, 35f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0));

            //iTextSharp.text.Font titles10 = new iTextSharp.text.Font(fontTitleSub, 10f, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0));

            //iTextSharp.text.Font titles12 = new iTextSharp.text.Font(fontTitleSub, 12f, iTextSharp.text.Font.BOLD, new BaseColor(0, 127, 0));

            //iTextSharp.text.Font paragraph = new iTextSharp.text.Font(fontTitleSub, 10f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));

            //iTextSharp.text.Font theader = new iTextSharp.text.Font(fontTitle, 12f, iTextSharp.text.Font.BOLD, new BaseColor(140, 40, 74));

            //doc.Add(Chunk.NEWLINE);


            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(_env.WebRootPath, "images", "LogoTotSp.png"));
            //float proporcion = logo.Width / logo.Height;
            //logo.ScaleAbsoluteWidth(200);
            //logo.ScaleAbsoluteHeight(40 * proporcion);
            //logo.SetAbsolutePosition(0, 567f);

            //PdfPTable myTable = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100f };
            //myTable.AddCell(new PdfPCell(logo) { Border = 0, Rowspan = 4, VerticalAlignment = Element.ALIGN_MIDDLE });
            //myTable.AddCell(new PdfPCell(new Phrase("Gobernación de Arauca", paragraph)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            //myTable.AddCell(new PdfPCell(new Phrase("Secretaría Departamental de Educación", paragraph)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            //myTable.AddCell(new PdfPCell(new Phrase("Calle 20 con carrera 21", paragraph)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            //myTable.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MMM/yyyy"), paragraph)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });


            //doc.Add(myTable);

            //doc.Add(new Phrase(" "));




            //myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };
            //myTable.AddCell(new PdfPCell(new Phrase($"Listado clases de categorías", titles12)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            //doc.Add(myTable);

            //doc.Add(new Phrase("\n"));
            //myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            //PdfPCell pdfPCell = new PdfPCell(new Phrase("Descripción", theader)) { Border = 0, BorderColorTop = new BaseColor(195, 195, 195), BorderWidthTop = 1.99f, HorizontalAlignment = Element.ALIGN_LEFT, PaddingTop = 8f, PaddingBottom = 8f };

            //myTable.AddCell(pdfPCell);
            //doc.Add(myTable);

            //myTable = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            //List<CategoryType> model = await _context.CategoryTypes.ToListAsync();
            //int cont = 1;
            //foreach (var item in model)
            //{

            //    pdfPCell.Phrase = new Phrase(item.Name, paragraph);
            //    pdfPCell.Border = 0;
            //    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    if (cont % 2 == 0)
            //    {
            //        pdfPCell.BackgroundColor = BaseColor.WHITE;
            //    }
            //    else
            //    {
            //        pdfPCell.BackgroundColor = new BaseColor(248, 248, 248);
            //    }

            //    //myTable.AddCell(new PdfPCell(new Phrase(item.Name, paragraph)) { Border = 0, BorderWidthTop = 1.99f, BorderColorTop = new BaseColor(195, 195, 195), PaddingTop = 10f, HorizontalAlignment = Element.ALIGN_LEFT });
            //    //myTable.AddCell(new PdfPCell(new Phrase(item.Name, paragraph)) { Border = 0, BorderWidthTop = 1.99f, BorderColorTop = new BaseColor(44, 117, 185), PaddingTop = 10f, HorizontalAlignment = Element.ALIGN_LEFT });

            //    myTable.AddCell(pdfPCell);

            //    cont++;
            //}


            //doc.Add(myTable);

            //doc.Close();

            //write.Close();
        //    MemoryStream ms = await _pdfDocument.ReportAsync("clases de categorías");

        //    return ms;
        //}
    }
}
