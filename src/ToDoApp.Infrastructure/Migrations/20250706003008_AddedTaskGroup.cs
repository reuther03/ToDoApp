using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTaskGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskGroupId",
                table: "ToDoTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoTasks_TaskGroupId",
                table: "ToDoTasks",
                column: "TaskGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoTasks_TaskGroups_TaskGroupId",
                table: "ToDoTasks",
                column: "TaskGroupId",
                principalTable: "TaskGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoTasks_TaskGroups_TaskGroupId",
                table: "ToDoTasks");

            migrationBuilder.DropTable(
                name: "TaskGroups");

            migrationBuilder.DropIndex(
                name: "IX_ToDoTasks_TaskGroupId",
                table: "ToDoTasks");

            migrationBuilder.DropColumn(
                name: "TaskGroupId",
                table: "ToDoTasks");
        }
    }
}
