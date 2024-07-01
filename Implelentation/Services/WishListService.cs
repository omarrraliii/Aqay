
using aqay_apis.Context;
using aqay_apis.Models;
using Microsoft.EntityFrameworkCore;

namespace aqay_apis;

public class WishListService : IWishListService
{
    private ApplicationDbContext _context;
    private GlobalVariables _globalVariables;
    public WishListService(ApplicationDbContext context, GlobalVariables globalVariables)
    {
        _context = context;
        _globalVariables = globalVariables;
    }

    public async Task<WishList> AddProductToWishList(int id, int productId)
    {
        var wishList= await _context.WishLists.Include(w=>w.Products)
                                        .FirstOrDefaultAsync(w=>w.Id==id);
        if (wishList == null)
        {
            return null;
            throw new Exception($"{id} not found");
        }
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return null;
            throw new Exception($"{id} not found");
        }
        wishList.Products.Add(product);
        await _context.SaveChangesAsync();
        return wishList;
    }

    public async Task<WishList> CreateWishListAsync(Consumer consumer)
    {
        var wishList=new WishList
        {
            Consumer=consumer
        };
        _context.WishLists.Add(wishList);
        await _context.SaveChangesAsync();
        return wishList;
    }

    public async Task<bool> DeleteWishListAsync(int id)
    {
        var wishList = await _context.WishLists.FindAsync(id);
        if (wishList == null)
        {
            return false;
            throw new Exception($"{id} was not found.");
        }
        _context.WishLists.Remove(wishList);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PaginatedResult<Product>> GetWishListByIdAsync(int id,int pageindex)
    {
        var wishList=await _context.WishLists.Include(w=>w.Products)
                                            .FirstOrDefaultAsync(w=>w.Id==id);
        if (wishList == null)
        {
            throw new Exception($"{id} was not found.");
        }
        var products=wishList.Products.OrderByDescending(p=>p.Name)
                        .Skip((pageindex-1)*_globalVariables.PageSize)
                        .Take(_globalVariables.PageSize)
                        .ToList();
        var productCount=wishList.Products.Count;
        var PaginatedResult=new PaginatedResult<Product>
        {
            Items=products,
            TotalCount=productCount,
            HasMoreItems=(pageindex*_globalVariables.PageSize)<productCount
        };
        return PaginatedResult;
    }

    public async Task<WishList> RemoveProductFromWishList(int id, int productId)
    {
        var wishList=await _context.WishLists.Include(w=>w.Products)
                                                .FirstOrDefaultAsync(w=>w.Id == id);
        if(wishList==null)
        {
            return null;
            throw new Exception ($"{id} was not found");
        }
        var product = wishList.Products.FirstOrDefault(p=>p.Id==productId);
        if (product == null)
        {
            return null;
            throw new Exception ($"product {id} was not found");
        }
        wishList.Products.Remove(product);
        await _context.SaveChangesAsync();
        return wishList;
    }
}
