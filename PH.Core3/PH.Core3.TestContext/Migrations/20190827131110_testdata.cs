using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PH.Core3.TestContext.Migrations
{
    public partial class testdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedTransactionId = table.Column<long>(nullable: true),
                    CreatedTransactionId = table.Column<long>(nullable: false),
                    UpdatedTransactionId = table.Column<long>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Data_transaction_audit_CreatedTransactionId",
                        column: x => x.CreatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Data_transaction_audit_DeletedTransactionId",
                        column: x => x.DeletedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Data_transaction_audit_UpdatedTransactionId",
                        column: x => x.UpdatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Data_CreatedTransactionId",
                table: "Data",
                column: "CreatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Data_DeletedTransactionId",
                table: "Data",
                column: "DeletedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Data_UpdatedTransactionId",
                table: "Data",
                column: "UpdatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Data_Id_Deleted_CreatedTransactionId_UpdatedTransactionId_DeletedTransactionId",
                table: "Data",
                columns: new[] { "Id", "Deleted", "CreatedTransactionId", "UpdatedTransactionId", "DeletedTransactionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Data");
        }
    }
}
