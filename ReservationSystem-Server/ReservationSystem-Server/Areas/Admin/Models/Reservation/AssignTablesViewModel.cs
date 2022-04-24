namespace ReservationSystem_Server.Areas.Admin.Models.Reservation;

public class AssignTablesViewModel
{
    public int ReservationId { get; set; }
    public int SittingId { get; set; }

    public List<Table> Tables { get; set; } = null!;
    
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsAssigned { get; set; }
    }
}