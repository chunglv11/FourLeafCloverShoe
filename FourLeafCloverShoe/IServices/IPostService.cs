using FourLeafCloverShoe.Share.Models;

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
    }
}
