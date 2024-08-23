using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLeafCloverShoe.IServices
{
    public interface IPostService
    {
        public Task<bool> Add(Post obj);
        public Task<bool> AddMany(List<Post> lstobj);
        public Task<bool> Update(Post obj);
        public Task<bool> Delete(Guid Id);
        public Task<Post> GetById(Guid Id);
        public Task<List<Post>> Gets();
        public Task<List<Post>> GetsNoti();
        public Task<List<Post>> Getssell();
        public Task<List<Post>> GetLatestPosts(int count);
        public Task<List<Post>> Getsanotherpost(Guid id, bool status);
        }
}
