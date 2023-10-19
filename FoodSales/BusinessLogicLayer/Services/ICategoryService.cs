﻿using BusinessLogicLayer.Model;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface ICategoryService
    {
        Task<List<SelectListModel>> GetCategorysAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(RegionDto model);
        Task<bool> UpdateAsync(RegionUpdate model);
        Task<bool> DeleteAsync(Guid id);
    }
}