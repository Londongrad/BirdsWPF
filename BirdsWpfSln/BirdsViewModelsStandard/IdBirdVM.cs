using BirdsCommonStandard;

namespace BirdsViewModels
{
    public class IdBirdVM : IdDtoVM<Bird>
    {
        public IdBirdVM(int id)
            : base(id)
        { }

        public IdBirdVM(Bird dto)
            : base(dto)
        { }
    }
}
