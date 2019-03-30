using Microsoft.EntityFrameworkCore.Migrations;

namespace Cocoa.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Poses",
                columns: table => new
                {
                    PoseId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Delay = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poses", x => x.PoseId);
                });

            migrationBuilder.CreateTable(
                name: "Sequences",
                columns: table => new
                {
                    SequenceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequences", x => x.SequenceId);
                });

            migrationBuilder.CreateTable(
                name: "Joints",
                columns: table => new
                {
                    JointId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServoId = table.Column<int>(nullable: false),
                    Angle = table.Column<double>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    PoseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Joints", x => x.JointId);
                    table.ForeignKey(
                        name: "FK_Joints_Poses_PoseId",
                        column: x => x.PoseId,
                        principalTable: "Poses",
                        principalColumn: "PoseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SequencePoses",
                columns: table => new
                {
                    SequencePoseId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: false),
                    PoseId = table.Column<int>(nullable: false),
                    SequenceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequencePoses", x => x.SequencePoseId);
                    table.ForeignKey(
                        name: "FK_SequencePoses_Poses_PoseId",
                        column: x => x.PoseId,
                        principalTable: "Poses",
                        principalColumn: "PoseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SequencePoses_Sequences_SequenceId",
                        column: x => x.SequenceId,
                        principalTable: "Sequences",
                        principalColumn: "SequenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Joints_PoseId",
                table: "Joints",
                column: "PoseId");

            migrationBuilder.CreateIndex(
                name: "IX_SequencePoses_PoseId",
                table: "SequencePoses",
                column: "PoseId");

            migrationBuilder.CreateIndex(
                name: "IX_SequencePoses_SequenceId",
                table: "SequencePoses",
                column: "SequenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Joints");

            migrationBuilder.DropTable(
                name: "SequencePoses");

            migrationBuilder.DropTable(
                name: "Poses");

            migrationBuilder.DropTable(
                name: "Sequences");
        }
    }
}
