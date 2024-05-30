// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using FourLeafCloverShoe.Share.Models;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.IServices
{
    public interface IUserVoucherService
    {
        public Task<bool> Add(UserVoucher obj);
        public Task<bool> AddMany(List<UserVoucher> lstobj);
        public Task<bool> Update(UserVoucher obj);
        public Task<bool> Delete(Guid Id);
        public Task<UserVoucher> GetById(Guid Id);
        public Task<List<UserVoucher>> GetByUserId(string userId);
        public Task<List<UserVoucher>> GetByVoucherId(Guid voucherId);
        public Task<List<UserVoucher>> Gets();
    }
}
