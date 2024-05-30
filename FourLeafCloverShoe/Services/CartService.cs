using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class CartService : ICartService
    {
        private readonly MyDbContext _myDbContext;

        public CartService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Cart obj)
        {
            try
            {
                await _myDbContext.Carts.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Cart> lstobj)
        {
            try
            {
                await _myDbContext.Carts.AddRangeAsync(lstobj);
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
                    _myDbContext.Carts.Remove(obj);
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

        public async Task<Cart> GetById(Guid Id)
        {
            try
            {
                var obj = await _myDbContext.Carts.FindAsync(Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Cart();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Cart();
            }
        }

        public async Task<Cart> GetByUserId(string userId)
        {
            try
            {
                var lstobj = await _myDbContext.Carts.ToListAsync();
                var cart = lstobj.FirstOrDefault(c=>c.UserId==userId);  
                if (cart != null)
                {

                    return cart;
                }
                return new Cart();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Cart();
            }
        }

        public async Task<List<Cart>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Carts.ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Cart>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Cart>();
            }
        }

        public async Task<bool> Update(Cart obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.UserId = obj.UserId;
                    _myDbContext.Carts.Update(objFromDb);
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
    }
}