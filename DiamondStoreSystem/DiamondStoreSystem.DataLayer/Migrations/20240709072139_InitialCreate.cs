using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiamondStoreSystem.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    AccessoryID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Material = table.Column<int>(type: "int", nullable: false),
                    Style = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Block = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    UnitInStock = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.AccessoryID);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoyaltyPoint = table.Column<int>(type: "int", nullable: true),
                    Block = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    WorkingSchedule = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "Diamond",
                columns: table => new
                {
                    DiamondID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LabCreated = table.Column<int>(type: "int", nullable: false),
                    TablePercent = table.Column<double>(type: "float", nullable: false),
                    DepthPercent = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GIAReportNumber = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shape = table.Column<int>(type: "int", nullable: false),
                    CaratWeight = table.Column<double>(type: "float", nullable: false),
                    ColorGrade = table.Column<int>(type: "int", nullable: false),
                    ClarityGrade = table.Column<int>(type: "int", nullable: false),
                    CutGrade = table.Column<int>(type: "int", nullable: false),
                    PolishGrade = table.Column<int>(type: "int", nullable: false),
                    SymmetryGrade = table.Column<int>(type: "int", nullable: false),
                    FluoresceneGrade = table.Column<int>(type: "int", nullable: false),
                    Inscription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Block = table.Column<bool>(type: "bit", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diamond", x => x.DiamondID);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    DateOrdered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateReceived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeAssignID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PayMethod = table.Column<int>(type: "int", nullable: false),
                    Block = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_Accounts_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Accounts_EmployeeAssignID",
                        column: x => x.EmployeeAssignID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Block = table.Column<bool>(type: "bit", nullable: false),
                    AccessoryID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MainDiamondID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Accessories_AccessoryID",
                        column: x => x.AccessoryID,
                        principalTable: "Accessories",
                        principalColumn: "AccessoryID");
                    table.ForeignKey(
                        name: "FK_Products_Diamond_MainDiamondID",
                        column: x => x.MainDiamondID,
                        principalTable: "Diamond",
                        principalColumn: "DiamondID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VnPaymentResponses",
                columns: table => new
                {
                    VnpOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VnPayResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VnPaymentResponses", x => x.VnpOrderId);
                    table.ForeignKey(
                        name: "FK_VnPaymentResponses_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubDiamonds",
                columns: table => new
                {
                    SubDiamondID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDiamonds", x => x.SubDiamondID);
                    table.ForeignKey(
                        name: "FK_SubDiamonds_Diamond_SubDiamondID",
                        column: x => x.SubDiamondID,
                        principalTable: "Diamond",
                        principalColumn: "DiamondID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubDiamonds_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warranties",
                columns: table => new
                {
                    WarrantyID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Block = table.Column<bool>(type: "bit", nullable: false),
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warranties", x => x.WarrantyID);
                    table.ForeignKey(
                        name: "FK_Warranties_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Accessories",
                columns: new[] { "AccessoryID", "AccessoryName", "Block", "Brand", "Description", "ImageUrl", "Material", "Price", "SKU", "Style", "UnitInStock" },
                values: new object[] { "A001", "Gold Chain", false, "LuxuryBrand", "18k gold chain", "https://firebasestorage.googleapis.com/v0/b/diamond-store-system.appspot.com/o/365225250_603232441966413_6481642414490509587_n.jpg?alt=media&token=606330be-f47b-45cf-b157-1a9de4f1f91c", 1, 500.0, "GC001", 1, 10 });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountID", "Address", "Block", "DOB", "Email", "FirstName", "Gender", "JoinDate", "LastName", "LoyaltyPoint", "Password", "Phone", "Role", "WorkingSchedule" },
                values: new object[,]
                {
                    { "C001", "456 Customer Ave.", false, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "customer@example.com", "Regular", 0, new DateTime(2024, 7, 9, 14, 21, 38, 800, DateTimeKind.Local).AddTicks(7566), "Customer", 100, "473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8", "9876543210", 0, 0 },
                    { "S001", "123 Admin St.", false, new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", "Super", 1, new DateTime(2024, 7, 9, 14, 21, 38, 800, DateTimeKind.Local).AddTicks(7513), "Admin", 0, "473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8", "1234567890", 3, 1 },
                    { "S002", "123 Admin St.", false, new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "staff1@example.com", "Super", 1, new DateTime(2024, 7, 9, 14, 21, 38, 800, DateTimeKind.Local).AddTicks(7548), "Admin", 0, "473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8", "1234567890", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Diamond",
                columns: new[] { "DiamondID", "Block", "CaratWeight", "ClarityGrade", "ColorGrade", "CutGrade", "DepthPercent", "Description", "FluoresceneGrade", "GIAReportNumber", "Height", "ImageURL", "Inscription", "IssueDate", "LabCreated", "Length", "Origin", "PolishGrade", "Price", "SKU", "Shape", "SymmetryGrade", "TablePercent", "Width" },
                values: new object[,]
                {
                    { "D001", false, 1.0, 1, 1, 1, 62.0, "Brilliant cut diamond", 1, 123456, 4.0, "https://firebasestorage.googleapis.com/v0/b/diamond-store-system.appspot.com/o/GP1Lty9XgAAI3l-.jpg?alt=media&token=3d8c0b2e-37bd-44ec-a489-9dcc14482312", "GIA12345446", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 6.0, "Africa", 1, 500.0, "DIA001", 1, 1, 57.5, 6.0 },
                    { "D002", false, 1.0, 1, 1, 1, 62.0, "Brilliant cut diamond", 1, 123456, 4.0, "https://assets.entrepreneur.com/content/3x2/2000/20160305000536-diamond.jpeg", "GIA12345446", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 6.0, "Africa", 1, 500.0, "DIA001", 1, 1, 57.5, 6.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerID",
                table: "Order",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EmployeeAssignID",
                table: "Order",
                column: "EmployeeAssignID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AccessoryID",
                table: "Products",
                column: "AccessoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MainDiamondID",
                table: "Products",
                column: "MainDiamondID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderID",
                table: "Products",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_SubDiamonds_ProductID",
                table: "SubDiamonds",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_VnPaymentResponses_OrderId",
                table: "VnPaymentResponses",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Warranties_ProductID",
                table: "Warranties",
                column: "ProductID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubDiamonds");

            migrationBuilder.DropTable(
                name: "VnPaymentResponses");

            migrationBuilder.DropTable(
                name: "Warranties");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropTable(
                name: "Diamond");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
