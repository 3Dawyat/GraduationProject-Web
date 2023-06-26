using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BL.Migrations
{
    public partial class AddNextVisit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "Gender",
               table: "TbCustomers",
               type: "nvarchar(max)",
               nullable: true);
          
            migrationBuilder.AddColumn<string>(
                name: "InvoiceType",
                table: "TbSalesInvoices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextVisit",
                table: "TbSalesInvoices",
                type: "datetime",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "TbSalesInvoices");

            migrationBuilder.DropColumn(
                name: "NextVisit",
                table: "TbSalesInvoices");

            migrationBuilder.DropColumn(
               name: "Gender",
               table: "TbCustomers");
        }
    }
}
