using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7MVC.Models;
using Team7MVC.Repositories;
using Team7MVC.ViewModels;

namespace Team7MVC.Controllers
{
    public class ProductController : Controller
    {
        public readonly ProductRepository _repo;

        public ProductController()
        {
            _repo = new ProductRepository();
        }
        // GET: Product
        [HttpGet]
        public ActionResult Index(int? Id)
        {
            List<Products> products;
            if (Id == 1)
            {
                products = _repo.Getproducts(Id);
            }
            else if (Id == 2)
            {
                products = _repo.Getproducts(Id);
            }
            else if (Id == 3)
            {
                products = _repo.Getproducts(Id);
            }
            else
            {
                products = _repo.GetProducts();
            }

            return View(products);
        }

        [HttpPost]
        public ActionResult Index(string search, int? Year_s, int? Year_e, decimal? Price_s, decimal? Price_e, string[] Origin, string[] Category)
        {
            List<Products> products;

            if (search != null)
            {
                products = _repo.GetProducts(search);

            }
            else if (Year_e == null && Year_s != null)
            {
                products = _repo.GetProducts(Year_s);
            }
            else if (Year_s != null)
            {
                products = _repo.GetProducts(Year_s, Year_e);
            }
            else if (Price_e == null && Price_s != null)
            {
                products = _repo.GetProducts(Price_s);
            }
            else if (Price_s != null)
            {
                products = _repo.GetProducts(Price_s, Price_e);
            }
            else if (Origin != null)
            {
                products = _repo.GetProducts(Origin, 1);
            }
            else if (Category != null)
            {
                products = _repo.GetProducts(Category, 2);
            }
            else
            {
                products = _repo.GetProducts();
            }


            return View(products);
        }

        [HttpGet]
        public ActionResult ProductDetail(int Id)
        {
            var product = _repo.GetProductById(Id);
            return View(product);
        }

        [HttpPost]
        public ActionResult ProductDetail(int ProductId, int buyQty)
        {
            //var product = _repo.GetProductById(Id);

            //ShopListsViewModel shopLists = new ShopListsViewModel
            //{
            //    ProductId = product.ProductID,
            //    Price = product.UnitPrice,
            //    Quantity = buyQty
            //};

            _repo.CreateShoppingCartData(User.Identity.Name, ProductId, buyQty);

            return RedirectToAction("ShoppingCart");
        }

        [Authorize]
        public ActionResult ShoppingCart()
        {
            List<ShopListsViewModel> shopLists;
            shopLists = _repo.ShopList(User.Identity.Name);

            return View(shopLists);
        }

        [HttpPost]
        public ActionResult ShoppingCart(string nothing)
        {
            return RedirectToAction("Payment");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Payment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel();
            List<ShopListsViewModel> shopList;
            shopList = _repo.ShopList(User.Identity.Name);
            paymentViewModel.shopList = shopList;

            return View(paymentViewModel);
        }

        [HttpPost]
        public ActionResult Payment(/*DateTime RequiredDate,*/ string BillFirstName, string BillLastName, string BillPhone, string BillCity, string BillAddress, string ShipFirstName, string ShipLastName, string ShipPhone, string ShipCity, string ShipAddress, string PayWay, decimal Freight, string CreditCardNo4, string CreditCardNo8, string CreditCardNo12, string CreditCardNo16, string CreditCardMM, string CreditCardYY, int? CreditCardCSC, string IdentityCard)
        {
            CustomerPayment customerPayment;

            if (string.IsNullOrEmpty(CreditCardNo4) && string.IsNullOrEmpty(CreditCardNo8) && string.IsNullOrEmpty(CreditCardNo12) && string.IsNullOrEmpty(CreditCardNo16) && string.IsNullOrEmpty(CreditCardMM) && string.IsNullOrEmpty(CreditCardYY) && CreditCardCSC == null && string.IsNullOrEmpty(IdentityCard))
            {
                if (string.IsNullOrEmpty(BillFirstName) && string.IsNullOrEmpty(BillLastName) && string.IsNullOrEmpty(BillCity) && string.IsNullOrEmpty(BillAddress) && string.IsNullOrEmpty(BillPhone))
                {
                    customerPayment = new CustomerPayment()
                    {
                        OrderDate = DateTime.Now,
                        //RequiredDate = DateTime.Now,
                        BillName = ShipFirstName + ShipLastName,
                        BillPhone = ShipPhone,
                        BillCity = ShipCity,
                        BillAddress = ShipAddress,
                        ShipperID = 1,
                        ShipName = ShipFirstName + ShipLastName,
                        ShipPhone = ShipPhone,
                        ShipAddress = ShipAddress,
                        ShipCity = ShipCity,
                        PayWay = "ATM",
                        PayDate = DateTime.Now,
                        Freight= Freight
                    };
                }
                else
                {
                    customerPayment = new CustomerPayment()
                    {
                        OrderDate = DateTime.Now,
                        //RequiredDate = DateTime.Now,
                        BillName = BillFirstName + BillLastName,
                        BillPhone = BillPhone,
                        BillCity = BillCity,
                        BillAddress = BillAddress,
                        ShipperID = 1,
                        ShipName = ShipFirstName + ShipLastName,
                        ShipPhone = ShipPhone,
                        ShipAddress = ShipAddress,
                        ShipCity = ShipCity,
                        PayWay = "ATM",
                        PayDate = DateTime.Now,
                        Freight = Freight
                    };
                }

            }
            else
            {
                if (string.IsNullOrEmpty(BillFirstName) && string.IsNullOrEmpty(BillLastName) && string.IsNullOrEmpty(BillCity) && string.IsNullOrEmpty(BillAddress) && string.IsNullOrEmpty(BillPhone))
                {
                    customerPayment = new CustomerPayment()
                    {
                        OrderDate = DateTime.Now,
                        //RequiredDate = DateTime.Now,
                        BillName = ShipFirstName + ShipLastName,
                        BillPhone = ShipPhone,
                        BillCity = ShipCity,
                        BillAddress = ShipAddress,
                        ShipperID = 1,
                        ShipName = ShipFirstName + ShipLastName,
                        ShipPhone = ShipPhone,
                        ShipAddress = ShipAddress,
                        ShipCity = ShipCity,
                        PayWay = "CreditCard",
                        CreditCardNo = CreditCardNo4 + CreditCardNo8 + CreditCardNo12 + CreditCardNo16,
                        CreditCardCSC = CreditCardCSC,
                        CreditCardDate = CreditCardMM + CreditCardYY,
                        IdentityCard = IdentityCard,
                        PayDate = DateTime.Now,
                        Freight = Freight
                    };
                }
                else
                {
                    customerPayment = new CustomerPayment()
                    {
                        OrderDate = DateTime.Now,
                        //RequiredDate = DateTime.Now,
                        BillName = BillFirstName + BillLastName,
                        BillPhone = BillPhone,
                        BillCity = BillCity,
                        BillAddress = BillAddress,
                        ShipperID = 1,
                        ShipName = ShipFirstName + ShipLastName,
                        ShipPhone = ShipPhone,
                        ShipAddress = ShipAddress,
                        ShipCity = ShipCity,
                        PayWay = "CreditCard",
                        CreditCardNo = CreditCardNo4 + CreditCardNo8 + CreditCardNo12 + CreditCardNo16,
                        CreditCardCSC = CreditCardCSC,
                        CreditCardDate = CreditCardMM + CreditCardYY,
                        IdentityCard = IdentityCard,
                        PayDate = DateTime.Now,
                        Freight = Freight
                    };
                }



            }

            PaymentViewModel paymentViewModel = new PaymentViewModel()
            {
                customerPayment = customerPayment
            };
            var CustomerID = _repo.GetCustomerID(User.Identity.Name);
            _repo.Payment(paymentViewModel, CustomerID);

            return RedirectToAction("OrderDetail", "OrderDetail");
        }
    }
}