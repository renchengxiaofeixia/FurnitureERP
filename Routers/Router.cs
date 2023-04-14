
using Microsoft.AspNetCore.Builder;

namespace FurnitureERP.Routers
{
    public static class Router
    {
        public static IApplicationBuilder Use(IApplicationBuilder builder)
        {
            var app = builder as WebApplication;
            app.MapPost("/signin", AuthController.Signin);
            app.MapPost("/signup", AuthController.Signup);
            app.MapPost("/smscode/{mobileNo}", AuthController.SendSmsCode);


            app.MapPost("/user", UserController.Create);
            app.MapGet("/users", UserController.Get);
            app.MapGet("/user/{id}", UserController.Single);
            app.MapPut("/user/{id}", UserController.Edit);
            app.MapDelete("/user/{id}", UserController.Delete);


            app.MapPost("/role", RoleController.Create);
            app.MapGet("/roles", RoleController.Get);
            app.MapGet("/role/{id}", RoleController.Single);
            app.MapPut("/role/{id}", RoleController.Edit);
            app.MapDelete("/role/{id}", RoleController.Delete);
            app.MapPost("/userrole", RoleController.CreateUserRole);

            app.MapPost("/propconfig", CustomPropertyController.CreatePropConfig);
            app.MapGet("/propconfig", CustomPropertyController.SinglePropConfig);
            app.MapGet("/modules/", CustomPropertyController.GetMods);


            //app.MapPost("/customer", CustomerController.Create);
            //app.MapGet("/customers", CustomerController.Get);
            //app.MapGet("/customer/{id}", CustomerController.Single);
            //app.MapPut("/customer/{id}", CustomerController.Edit);
            //app.MapDelete("/customer/{id}", CustomerController.Delete);
            //app.MapGet("/contactrecords/{id}", CustomerController.GetContactRecords);
            //app.MapGet("/customer/orders/{customerNo}", CustomerController.GetOrders);


            //app.MapPost("/customercontact", CustomerContactController.Create);
            //app.MapGet("/customercontacts", CustomerContactController.Get);
            //app.MapGet("/customercontact/{id}", CustomerContactController.Single);
            //app.MapPut("/customercontact/{id}", CustomerContactController.Edit);
            //app.MapDelete("/customercontact/{id}", CustomerContactController.Delete);


            app.MapPost("/item", ItemController.Create);
            app.MapGet("/items", ItemController.Get);
            app.MapGet("/items/page", ItemController.Page);
            app.MapGet("/item/{id}", ItemController.Single);
            app.MapPut("/item/{id}", ItemController.Edit);
            app.MapDelete("/item/{id}", ItemController.Delete);
            app.MapPost("/item/import", ItemController.Import);

            app.MapPost("/item/upload", ItemController.Upload);
            //app.MapGet("/prod/suppliers/{id}", ProdController.GetSuppliers);
            //app.MapGet("/prod/inventorys/{prodNo}", ProdController.GetWarehouseProdInfos);
            //app.MapGet("/prod/purchases/{prodNo}", ProdController.GetPurchases);

            //app.MapPost("/prodcat", ProdCategoryController.Create);
            //app.MapGet("/prodcats", ProdCategoryController.Get);
            //app.MapGet("/prodcat/{id}", ProdCategoryController.Single);
            //app.MapPut("/prodcat/{id}", ProdCategoryController.Edit);
            //app.MapDelete("/prodcat/{id}", ProdCategoryController.Delete);

            app.MapPost("/supplier", SupplierController.Create);
            app.MapGet("/suppliers", SupplierController.Get);
            app.MapGet("/supplier/{id}", SupplierController.Single);
            app.MapPut("/supplier/{id}", SupplierController.Edit);
            app.MapDelete("/supplier/{id}", SupplierController.Delete);
            app.MapGet("/supplier/items/{id}", SupplierController.GetSuppItems);
            app.MapPost("/supplier/import", SupplierController.Import);

            //app.MapGet("/supplier/prodinfos/{id}", SupplierController.GetSupplyProds);
            //app.MapGet("/supplier/purchases/{id}", SupplierController.GetPurchases);

            app.MapPost("/purchase", PurchaseController.Create);
            app.MapGet("/purchases/page", PurchaseController.Page);
            app.MapGet("/purchase/{id}", PurchaseController.Single);
            app.MapPut("/purchase/{id}", PurchaseController.Edit);
            app.MapDelete("/purchase/{id}", PurchaseController.Delete);
            app.MapGet("/purchase/items/{id}", PurchaseController.GetPurchaseProdInfos);
            app.MapPut("/purchase/audit/{id}", PurchaseController.Audit);
            app.MapPut("/purchase/unaudit/{id}", PurchaseController.UnAudit);
            app.MapPut("/purchase/cancel/{id}", PurchaseController.CancelPurchaseItem);

            //app.MapPost("/purchase/pay", PurchaseController.CreatePurchasePay);
            //app.MapGet("/purchase/pays/{purchaseId}", PurchaseController.GetPurchasePayments);
            //app.MapGet("/purchase/pay/{id}", PurchaseController.SinglePurchasePay);
            //app.MapPut("/purchase/pay/{id}", PurchaseController.EditPurchasePay);
            //app.MapDelete("/purchase/pay/{id}", PurchaseController.DeletePurchasePayment);


            app.MapPost("/storage", StorageController.Create);
            app.MapGet("/storages/page", StorageController.Page);
            app.MapGet("/storage/{id}", StorageController.Single);
            app.MapPut("/storage/{id}", StorageController.Edit);
            app.MapDelete("/storage/{id}", StorageController.Delete);
            app.MapGet("/storage/items/{id}", StorageController.GetStorageProdInfos);
            app.MapPut("/storage/audit/{id}", StorageController.Audit);
            app.MapPut("/storage/unaudit/{id}", StorageController.UnAudit);


            app.MapGet("/inventory", InventoryController.GetInventories);
            app.MapGet("/inventory/count", InventoryController.GetInventoriesCountForWareName);
            app.MapPost("/inventory/move", InventoryController.MoveInventoryItems);
            app.MapPost("/inventory/adjust", InventoryController.AdjustInventoryItems);

            //app.MapPost("/check", CheckController.Create);
            //app.MapGet("/check", CheckController.Get);
            //app.MapGet("/check/{id}", CheckController.Single);
            //app.MapPut("/check/{id}", CheckController.Edit);
            //app.MapDelete("/check/{id}", CheckController.Delete);
            //app.MapGet("/check/prodinfos/{id}", CheckController.GetCheckProdInfos);
            //app.MapPut("/check/audit/{id}", CheckController.Audit);


            //app.MapPost("/transfer", TransferController.Create);
            //app.MapGet("/transfer", TransferController.Get);
            //app.MapGet("/transfer/{id}", TransferController.Single);
            //app.MapPut("/transfer/{id}", TransferController.Edit);
            //app.MapDelete("/transfer/{id}", TransferController.Delete);
            //app.MapGet("/transfer/prodinfos/{id}", TransferController.GetTransferProdInfos);
            //app.MapPut("/transfer/audit/{id}", TransferController.Audit);



            //app.MapPost("/order", OrderController.Create);
            //app.MapGet("/orders", OrderController.Get);
            //app.MapGet("/order/{id}", OrderController.Single);
            //app.MapPut("/order/{id}", OrderController.Edit);
            //app.MapDelete("/order/{id}", OrderController.Delete);
            //app.MapGet("/order/prodinfos/{id}", OrderController.GetOrderProdInfos);
            //app.MapPut("/order/audit/{id}", OrderController.Audit);
            //app.MapPut("/order/unaudit/{id}", OrderController.UnAudit);


            //app.MapPost("/order/pay", OrderController.CreateOrderPay);
            //app.MapGet("/order/pays/{orderId}", OrderController.GetOrderPayments);
            //app.MapGet("/order/pay/{id}", OrderController.SingleOrderPay);
            //app.MapPut("/order/pay/{id}", OrderController.EditOrderPay);
            //app.MapDelete("/order/pay/{id}", OrderController.DeleteOrderPayment);


            //app.MapPost("/menu", MenuController.Create);
            //app.MapGet("/menus", MenuController.Get);
            //app.MapGet("/menu/{id}", MenuController.Single);
            //app.MapPut("/menu/{id}", MenuController.Edit);
            //app.MapDelete("/menu/{id}", MenuController.Delete);

            app.MapPost("/warehouse", WarehouseController.Create);
            app.MapGet("/warehouses", WarehouseController.Get);
            app.MapGet("/warehouse/{id}", WarehouseController.Single);
            app.MapPut("/warehouse/{id}", WarehouseController.Edit);
            app.MapDelete("/warehouse/{id}", WarehouseController.Delete);

            return app;
        }

    }
}
