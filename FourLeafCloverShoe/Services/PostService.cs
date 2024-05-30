using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.Services
{
    public class PostService : IPostService
    {
        private readonly MyDbContext _myDbContext;

        public PostService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> Add(Post obj)
        {
            try
            {
                obj.CreateAt = DateTime.Now;
                await _myDbContext.Posts.AddAsync(obj);
                await _myDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddMany(List<Post> lstobj)
        {
            try
            {
                await _myDbContext.Posts.AddRangeAsync(lstobj);
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
                    _myDbContext.Posts.Remove(obj);
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

        public async Task<Post> GetById(Guid Id)
        {
            try
            {
                var lstobj = await _myDbContext.Posts
                    .Include(c => c.Users)
                    .ToListAsync();
                var obj =  lstobj.FirstOrDefault(c=>c.Id==Id);
                if (obj != null)
                {

                    return obj;
                }
                return new Post();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Post();
            }
        }

        public async Task<List<Post>> Gets()
        {
            try
            {
                var obj = await _myDbContext.Posts
                    .Include(c => c.Users)
                    .ToListAsync();
                if (obj != null)
                {

                    return obj;
                }
                return new List<Post>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Post>();
            }
        }

        public async Task<bool> Update(Post obj)
        {
            try
            {
                var objFromDb = await GetById(obj.Id);
                if (obj != null)
                {
                    objFromDb.Tittle = obj.Tittle;
                    objFromDb.TittleImage = obj.TittleImage;
                    objFromDb.Contents = obj.Contents;
                    objFromDb.UpdateAt = DateTime.Now;
                    objFromDb.Description = obj.Description;
                    objFromDb.Status = obj.Status;
                    _myDbContext.Posts.Update(objFromDb);
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
