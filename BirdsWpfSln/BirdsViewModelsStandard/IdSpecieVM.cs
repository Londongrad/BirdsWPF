using BirdsCommonStandard;

namespace BirdsViewModels
{
    public class IdSpecieVM : IdDtoVM<Specie>
    {
        public IdSpecieVM(int id)
            : base(id)
        { }

        public IdSpecieVM(Specie dto)
            : base(dto)
        { }
    }
}
