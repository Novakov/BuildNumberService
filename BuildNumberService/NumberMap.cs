using DapperExtensions.Mapper;

namespace BuildNumberService
{
    public class NumberMap : DapperExtensions.Mapper.ClassMapper<Number>
    {
        public NumberMap()
        {
            this.Table("Numbers");
            this.Map(x => x.Key).Key(KeyType.Assigned);
            this.Map(x => x.LastVersion);
            this.Map(x => x.ObtainedAt);
        }
    }
}