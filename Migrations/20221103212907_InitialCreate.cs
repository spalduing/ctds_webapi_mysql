using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ctds_webapi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Address = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Cellphone = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Age = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Seniority = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    TableId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Reserved = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    Stalls = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.TableId);
                });

            migrationBuilder.CreateTable(
                name: "Waiter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Age = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Seniority = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waiter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    BillId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    TableId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    WaiterId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bill_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bill_Waiter_WaiterId",
                        column: x => x.WaiterId,
                        principalTable: "Waiter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detail_Bill",
                columns: table => new
                {
                    DetailBilId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    BillId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ManagerId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Dish = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Value = table.Column<double>(type: "BINARY_DOUBLE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail_Bill", x => x.DetailBilId);
                    table.ForeignKey(
                        name: "FK_Detail_Bill_Bill_BillId",
                        column: x => x.BillId,
                        principalTable: "Bill",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detail_Bill_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Address", "Cellphone", "LastName", "Name" },
                values: new object[,]
                {
                    { new Guid("80bb2ae8-d779-4247-a8d3-2270b4ec68d1"), "8th street", 3224657628L, "Danger", "Ron" },
                    { new Guid("80bb2ae8-d779-4247-a8d3-2270b4ec68d2"), "7th street", 3224657628L, "Doe", "John" },
                    { new Guid("80bb2ae8-d779-4247-a8d3-2270b4ec68d3"), "6th street", 3224657628L, "bar", "Foo" }
                });

            migrationBuilder.InsertData(
                table: "Manager",
                columns: new[] { "Id", "Age", "LastName", "Name", "Seniority" },
                values: new object[,]
                {
                    { new Guid("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 20, "Quiñones", "Juan", 0 },
                    { new Guid("2cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 20, "Brando", "Mike", 0 },
                    { new Guid("3cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 20, "Corleone", "Vito", 0 }
                });

            migrationBuilder.InsertData(
                table: "Table",
                columns: new[] { "TableId", "Name", "Reserved", "Stalls" },
                values: new object[,]
                {
                    { new Guid("b80c2655-5a22-4ba2-94a1-688a85d6d91b"), "1", true, 4 },
                    { new Guid("b80c2655-5a22-4ba2-94a1-688a85d6d92b"), "2", true, 6 },
                    { new Guid("b80c2655-5a22-4ba2-94a1-688a85d6d93b"), "3", true, 4 }
                });

            migrationBuilder.InsertData(
                table: "Waiter",
                columns: new[] { "Id", "Age", "LastName", "Name", "Seniority" },
                values: new object[,]
                {
                    { new Guid("133e9d8d-7bbc-4e23-93c4-cb8de085919f"), 28, "Mikella", "Michael", 1 },
                    { new Guid("233e9d8d-7bbc-4e23-93c4-cb8de085919f"), 28, "Warrior", "Hoarahlux", 0 },
                    { new Guid("333e9d8d-7bbc-4e23-93c4-cb8de085919f"), 28, "Guerrero", "Rex", 2 }
                });

            migrationBuilder.InsertData(
                table: "Bill",
                columns: new[] { "BillId", "CreatedAt", "CustomerId", "TableId", "WaiterId" },
                values: new object[,]
                {
                    { new Guid("ef91c997-d758-44c6-8b9f-a66d7027e21c"), new DateTime(2022, 11, 3, 16, 29, 6, 832, DateTimeKind.Local).AddTicks(9293), new Guid("80bb2ae8-d779-4247-a8d3-2270b4ec68d1"), new Guid("b80c2655-5a22-4ba2-94a1-688a85d6d91b"), new Guid("133e9d8d-7bbc-4e23-93c4-cb8de085919f") },
                    { new Guid("ef91c997-d758-44c6-8b9f-a66d7027e22c"), new DateTime(2022, 11, 3, 16, 29, 6, 832, DateTimeKind.Local).AddTicks(9349), new Guid("80bb2ae8-d779-4247-a8d3-2270b4ec68d1"), new Guid("b80c2655-5a22-4ba2-94a1-688a85d6d92b"), new Guid("233e9d8d-7bbc-4e23-93c4-cb8de085919f") },
                    { new Guid("ef91c997-d758-44c6-8b9f-a66d7027e23c"), new DateTime(2022, 11, 3, 16, 29, 6, 832, DateTimeKind.Local).AddTicks(9353), new Guid("80bb2ae8-d779-4247-a8d3-2270b4ec68d1"), new Guid("b80c2655-5a22-4ba2-94a1-688a85d6d93b"), new Guid("333e9d8d-7bbc-4e23-93c4-cb8de085919f") },
                    { new Guid("ef91c997-d758-44c6-8b9f-a66d7027e24c"), new DateTime(2022, 11, 3, 16, 29, 6, 832, DateTimeKind.Local).AddTicks(9356), new Guid("80bb2ae8-d779-4247-a8d3-2270b4ec68d2"), new Guid("b80c2655-5a22-4ba2-94a1-688a85d6d91b"), new Guid("133e9d8d-7bbc-4e23-93c4-cb8de085919f") },
                    { new Guid("ef91c997-d758-44c6-8b9f-a66d7027e25c"), new DateTime(2022, 11, 3, 16, 29, 6, 832, DateTimeKind.Local).AddTicks(9359), new Guid("80bb2ae8-d779-4247-a8d3-2270b4ec68d3"), new Guid("b80c2655-5a22-4ba2-94a1-688a85d6d93b"), new Guid("133e9d8d-7bbc-4e23-93c4-cb8de085919f") }
                });

            migrationBuilder.InsertData(
                table: "Detail_Bill",
                columns: new[] { "DetailBilId", "BillId", "Dish", "ManagerId", "Value" },
                values: new object[,]
                {
                    { new Guid("d1d42909-a120-44b8-9fe0-ecc99d643161"), new Guid("ef91c997-d758-44c6-8b9f-a66d7027e21c"), 1, new Guid("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 4.4000000000000004 },
                    { new Guid("d1d42909-a120-44b8-9fe0-ecc99d643162"), new Guid("ef91c997-d758-44c6-8b9f-a66d7027e22c"), 4, new Guid("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 4.4000000000000004 },
                    { new Guid("d1d42909-a120-44b8-9fe0-ecc99d643164"), new Guid("ef91c997-d758-44c6-8b9f-a66d7027e24c"), 4, new Guid("3cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 4.4000000000000004 },
                    { new Guid("d1d42909-a120-44b8-9fe0-ecc99d643165"), new Guid("ef91c997-d758-44c6-8b9f-a66d7027e24c"), 4, new Guid("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 4.4000000000000004 },
                    { new Guid("d1d42909-a120-44b8-9fe0-ecc99d643166"), new Guid("ef91c997-d758-44c6-8b9f-a66d7027e22c"), 0, new Guid("2cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 4.4000000000000004 },
                    { new Guid("d1d42909-a120-44b8-9fe0-ecc99d643167"), new Guid("ef91c997-d758-44c6-8b9f-a66d7027e21c"), 2, new Guid("3cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 4.4000000000000004 },
                    { new Guid("d1d42909-a120-44b8-9fe0-ecc99d643168"), new Guid("ef91c997-d758-44c6-8b9f-a66d7027e24c"), 2, new Guid("2cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 4.4000000000000004 },
                    { new Guid("d1d42909-a120-44b8-9fe0-ecc99d643169"), new Guid("ef91c997-d758-44c6-8b9f-a66d7027e25c"), 3, new Guid("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"), 4.4000000000000004 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_CustomerId",
                table: "Bill",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_TableId",
                table: "Bill",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_WaiterId",
                table: "Bill",
                column: "WaiterId");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_Bill_BillId",
                table: "Detail_Bill",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_Bill_ManagerId",
                table: "Detail_Bill",
                column: "ManagerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detail_Bill");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "Waiter");
        }
    }
}
