using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class CartItemService : ICartItemItemService
    {
        private readonly MyDbContext _myDbContext;

        public CartItemService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(CartItem obj)
        {
            try
            {
                await _myDbContext.CartItems.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<CartItem> lstobj)
        {
            try
            {
                await _myDbContext.CartItems.AddRangeAsync(lstobj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                var obj = await GetById(Id);
                if (obj != null)
                {
                    _myDbContext.CartItems.Remove(obj);
                    await _myDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteByProductDetailId(Guid productDetailId)
        {
            try
            {
                var obj = await Gets();
                if (obj != null)
                {
                    _myDbContext.CartItems.Remove(obj.FirstOrDefault(c=>c.ProductDetailId==productDetailId));
                    await _myDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteMany(List<CartItem> lstobj)
        {
            try
            {
                 _myDbContext.CartItems.RemoveRange(lstobj);
                await _myDbContext.SaveChangesAsync();
                    return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<CartItem> GetById(Guid Id)
        {
            try
            {
                var lstobj = await _myDbContext.CartItems
                    .Include(c=>c.ProductDetails)
                        .ThenInclude(c=>c.Products)
                            .ThenInclude(c=>c.ProductImages)
                     .Include(c => c.ProductDetails)
                        .ThenInclude(c=>c.Size)
                         .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Colors)
                    .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Products)
                            .ThenInclude(c=>c.Brands)
                     .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Products)
                            .ThenInclude(c=>c.Categories)
                            .ToListAsync();
                var obj =  lstobj.FirstOrDefault(c=>c.Id==Id);
                if (obj != null)
                {

                    return obj;
                }
                return new CartItem();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new CartItem();
            }
        }

        public async Task<List<CartItem>> Gets()
        {
            try
            {
                var lstobj = await _myDbContext.CartItems
                    .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Products)
                            .ThenInclude(c => c.ProductImages)
                     .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Size)
                        .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Colors)
                    .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Products)
                            .ThenInclude(c => c.Brands)
                     .Include(c => c.ProductDetails)
                        .ThenInclude(c => c.Products)
                            .ThenInclude(c => c.Categories)
                            .ToListAsync();
                if (lstobj != null)
                {

                    return lstobj;
                }
                return new List<CartItem>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<CartItem>();
            }
        }

        public async Task<List<CartItem>> GetsByUserId(string userId)
        {
            
            try
            {
                var carts = await _myDbContext.Carts.ToListAsync();
                var cartId = carts.FirstOrDefault(c => c.UserId == userId)?.Id;
                
                var lstCartItems = await Gets();
                var cartItems = lstCartItems.Where(c => c.CartId == cartId).ToList();
                if (cartItems == null)
                {
                    return new List<CartItem>();
                }
                return cartItems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<CartItem>();
            }
        }

        public async Task<bool> Update(CartItem obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Quantity = obj.Quantity;
                    _myDbContext.CartItems.Update(objFromDb);
                    await _myDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateQuantity(Guid cartItemId, int? newquantity)
        {
            try
            {
                var cartitem = await _myDbContext.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);
                if (cartitem != null)
                {
                    cartitem.Quantity = newquantity;
                    _myDbContext.CartItems.Update(cartitem);
                    await _myDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
