﻿using FMAplication.Controllers.Common;
using FMAplication.Filters;
using FMAplication.Models.Menus;
using FMAplication.Services.Menus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FMAplication.Controllers.Menu
{
    [ApiController]
    //[JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class MenuController : BaseController
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuService _menu;
        public MenuController(ILogger<MenuController> logger, IMenuService menu)
        {
            _menu = menu;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetMenus()
        {
            try
            {
                var result = await _menu.GetMenusAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("get-active")]
        public async Task<IActionResult> GetActiveMenus()
        {
            try
            {
                var result = await _menu.GetActiveMenusAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("get-child")]
        public async Task<IActionResult> GetChildMenus()
        {
            try
            {
                var result = await _menu.GetChildMenusAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenu(int id)
        {
            try
            {
                var result = await _menu.GetMenuAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMenu([FromBody]MenuModel model)
        {
            try
            {
                var isExist = await _menu.IsMenuExistAsync(model.Name, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "Menu Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _menu.CreateAndUpdateParentAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMenu([FromBody]MenuModel model)
        {
            try
            {
                var isExist = await _menu.IsMenuExistAsync(model.Name, model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Name), "Menu Already Exist");
                }

                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _menu.UpdateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                var result = await _menu.DeleteAsync(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("assignRoleToMenu/{roleId}")]
        public async Task<IActionResult> AssignRoleToMenu([FromBody]List<MenuPermissionModel> model, int roleId)
        {
            try
            {
                var result = await _menu.AssignRoleToMenuAsync(model, roleId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("get-permission-menu/{roleId}")]
        public async Task<IActionResult> GetPermissionMenus(int roleId)
        {
            try
            {
                var result = await _menu.GetPermissionMenus(roleId);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}