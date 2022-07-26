﻿using TSSedaplanifica.Common;
using TSSedaplanifica.Data.Entities;
using TSSedaplanifica.Models;

namespace TSSedaplanifica.Helpers
{
    public interface ISolicitHelper
    {
        Task<Response> AddUpdateAsync(Solicit model);
        Task<Response> AddUpdateAsync(string id,string description);
        Task<Solicit> ByIdAsync(int id);
        Task<Solicit> ByIdAllAsync(int id);
        Task<Solicit> ByIdDetailAsync(int id);
        Task<List<Solicit>> ComboAsync();
        Task<Response> DeleteAsync(int id);
        Task<Response> RequestSendAsync(int id, string typeSolicitState);
        Task<List<Solicit>> ListStartStocktakingAsync();
        Task<List<SolicitViewModel>> ListAsync(string id);
        Task<SolicitConsolidateViewModel> ListConsolidateAsync(string id);

    }
}
