using AutoMapper;
using GeekShopping.ProductAPI.Data.ValeuObjects;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GeekShopping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySQLContext _contect;
        private IMapper _mapper;

        public ProductRepository(MySQLContext contect, IMapper mapper)
        {
            _contect = contect;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductVO>> FindAll()
        {
            List<Product> products = await _contect.Products.ToListAsync();
            return _mapper.Map<List<ProductVO>>(products);
        }

        public async Task<ProductVO> FindById(long id)
        {
            Product product = await _contect.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Create(ProductVO vo)
        {
            Product product = _mapper.Map<Product>(vo);
            _contect.Products.Add(product);
            await _contect.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }      
        public async Task<ProductVO> Update(ProductVO vo)
        {
            Product product = _mapper.Map<Product>(vo);
            _contect.Products.Update(product);
            await _contect.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }
        public async Task<bool> Delete(long id)
        {
            try
            {
                Product product = await _contect.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

                if (product == null)
                {
                    return false;
                }
                else
                {
                    _contect.Products.Remove(product);
                    await _contect.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception)
            {

                return false;
            }            
        }
    }
}
