using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdeToFood.Core;
using OdeToFood.Data;
using static OdeToFood.Core.Restaurant;

namespace OdeToFood.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(
            IRestaurantData restaurantData,
            IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
        }
        public IActionResult OnGet(int? restaurantId)
        {
            if (restaurantId.HasValue)
            {
                Restaurant = restaurantData.GetById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();
                // set any default values during an add here.
            }
            if (Restaurant == null)
                {
                    return RedirectToPage("./NotFound");
                }
            
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Restaurant.Id > 0)
            {
                Restaurant = restaurantData.Update(Restaurant);
                TempData["Message"] = "Restaurant updated.";
            }
            else
            {
                Restaurant = restaurantData.Add(Restaurant);
                TempData["Message"] = "Restaurant added.";
            }
            restaurantData.Commit();

            // PRG "Post Redirect Get"
            // To avoid leaving the user on the POST page where refreshes would re-post.
            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });           
        }
    }
}
