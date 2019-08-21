using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PH.Core3.TestContext.Migrations
{
    public partial class pippo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "PaoloTestHiLo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    TableName = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    KeyValues = table.Column<string>(nullable: true),
                    OldValues = table.Column<byte[]>(nullable: true),
                    NewValues = table.Column<byte[]>(nullable: true),
                    TransactionId = table.Column<string>(maxLength: 128, nullable: true),
                    Author = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 128, nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_audit",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    StrIdentifier = table.Column<string>(maxLength: 128, nullable: false),
                    Author = table.Column<string>(maxLength: 500, nullable: false),
                    UtcDateTime = table.Column<DateTime>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    MillisecDuration = table.Column<double>(nullable: false, defaultValue: 0.0),
                    Scopes = table.Column<string>(maxLength: 500, nullable: true),
                    CommitMessage = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_audit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transaction_audit_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    DeletedTransactionId = table.Column<long>(nullable: true),
                    CreatedTransactionId = table.Column<long>(nullable: false),
                    UpdatedTransactionId = table.Column<long>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_transaction_audit_CreatedTransactionId",
                        column: x => x.CreatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_transaction_audit_DeletedTransactionId",
                        column: x => x.DeletedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_transaction_audit_UpdatedTransactionId",
                        column: x => x.UpdatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    DeletedTransactionId = table.Column<long>(nullable: true),
                    CreatedTransactionId = table.Column<long>(nullable: false),
                    UpdatedTransactionId = table.Column<long>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_transaction_audit_CreatedTransactionId",
                        column: x => x.CreatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_transaction_audit_DeletedTransactionId",
                        column: x => x.DeletedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_transaction_audit_UpdatedTransactionId",
                        column: x => x.UpdatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    DeletedTransactionId = table.Column<long>(nullable: true),
                    CreatedTransactionId = table.Column<long>(nullable: false),
                    UpdatedTransactionId = table.Column<long>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EntityLevelLevel = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    RootId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 23, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_transaction_audit_CreatedTransactionId",
                        column: x => x.CreatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Categories_transaction_audit_DeletedTransactionId",
                        column: x => x.DeletedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_RootId",
                        column: x => x.RootId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Categories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Categories_transaction_audit_UpdatedTransactionId",
                        column: x => x.UpdatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Alberi",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    DeletedTransactionId = table.Column<long>(nullable: true),
                    CreatedTransactionId = table.Column<long>(nullable: false),
                    UpdatedTransactionId = table.Column<long>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EntityLevelLevel = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    RootId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alberi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alberi_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alberi_transaction_audit_CreatedTransactionId",
                        column: x => x.CreatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Alberi_transaction_audit_DeletedTransactionId",
                        column: x => x.DeletedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alberi_Alberi_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Alberi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alberi_Alberi_RootId",
                        column: x => x.RootId,
                        principalTable: "Alberi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Alberi_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Alberi_transaction_audit_UpdatedTransactionId",
                        column: x => x.UpdatedTransactionId,
                        principalTable: "transaction_audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_CategoryId",
                table: "Alberi",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_CreatedTransactionId",
                table: "Alberi",
                column: "CreatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_DeletedTransactionId",
                table: "Alberi",
                column: "DeletedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_ParentId",
                table: "Alberi",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_RootId",
                table: "Alberi",
                column: "RootId");

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_TenantId",
                table: "Alberi",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_UpdatedTransactionId",
                table: "Alberi",
                column: "UpdatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_EntityLevelLevel_RootId_ParentId",
                table: "Alberi",
                columns: new[] { "EntityLevelLevel", "RootId", "ParentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Alberi_Id_Deleted_CreatedTransactionId",
                table: "Alberi",
                columns: new[] { "Id", "Deleted", "CreatedTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_CreatedTransactionId",
                table: "AspNetRoles",
                column: "CreatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_DeletedTransactionId",
                table: "AspNetRoles",
                column: "DeletedTransactionId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_UpdatedTransactionId",
                table: "AspNetRoles",
                column: "UpdatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_Id_Deleted_CreatedTransactionId",
                table: "AspNetRoles",
                columns: new[] { "Id", "Deleted", "CreatedTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedTransactionId",
                table: "AspNetUsers",
                column: "CreatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DeletedTransactionId",
                table: "AspNetUsers",
                column: "DeletedTransactionId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UpdatedTransactionId",
                table: "AspNetUsers",
                column: "UpdatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Id_Deleted_CreatedTransactionId",
                table: "AspNetUsers",
                columns: new[] { "Id", "Deleted", "CreatedTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedTransactionId",
                table: "Categories",
                column: "CreatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DeletedTransactionId",
                table: "Categories",
                column: "DeletedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RootId",
                table: "Categories",
                column: "RootId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TenantId",
                table: "Categories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UpdatedTransactionId",
                table: "Categories",
                column: "UpdatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_EntityLevelLevel_RootId_ParentId",
                table: "Categories",
                columns: new[] { "EntityLevelLevel", "RootId", "ParentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Id_Deleted_CreatedTransactionId",
                table: "Categories",
                columns: new[] { "Id", "Deleted", "CreatedTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_transaction_audit_TenantId",
                table: "transaction_audit",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_audit_Id_StrIdentifier_Author_UtcDateTime_Timestamp",
                table: "transaction_audit",
                columns: new[] { "Id", "StrIdentifier", "Author", "UtcDateTime", "Timestamp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alberi");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "transaction_audit");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropSequence(
                name: "PaoloTestHiLo");
        }
    }
}
