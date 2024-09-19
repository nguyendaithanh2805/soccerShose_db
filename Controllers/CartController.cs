﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerShoesShop.Areas.Admin.Services;
using SoccerShoesShop.Common;
using SoccerShoesShop.Models;
using SoccerShoesShop.Services;

namespace SoccerShoesShop.Controllers
{
    [Authorize("UserOnly")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly GetCurrentUser _getCurrentUser;
        private readonly IAccountService _accountService;

        public CartController(IProductService productService, ICartService cartService, GetCurrentUser getCurrentUser, IAccountService accountService)
        {
            _productService = productService;
            _cartService = cartService;
            _getCurrentUser = getCurrentUser;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var username = _getCurrentUser.GetUsername();
            var carts = await _cartService.GetCartByUsernameAsync(username);
            return View(carts);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, Cart cart)
        {
            try
            {
                var userId = _getCurrentUser.GetUserId();
                await _cartService.AddOrUpdateCartAsync(userId, productId, quantity);
                return RedirectToAction("Index", "Menu");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("Error");
            }
        }
    }
}
