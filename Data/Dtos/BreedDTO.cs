namespace tcc_mypet_back.Data.Dtos
{
    public class BreedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdministratorId { get; set; }
        public int AnimalTypeId { get; set; }
    }
}
