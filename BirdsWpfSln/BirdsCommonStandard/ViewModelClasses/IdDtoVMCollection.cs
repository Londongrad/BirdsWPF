using System;
using System.Collections.Generic;
using System.Reflection;

namespace BirdsCommonStandard
{
    public partial class IdDtoVMCollection<TIdDto, TIdDtoVM> : ReadOnlySyncedObservableCollection<TIdDtoVM> where TIdDto : IdDto where TIdDtoVM : IdDtoVM<TIdDto>
    {
        private readonly Dictionary<int, TIdDtoVM> dict = new Dictionary<int, TIdDtoVM>();
        private readonly SyncedObservableCollection<TIdDtoVM> list = new SyncedObservableCollection<TIdDtoVM>();
        private readonly Func<TIdDto, TIdDtoVM> creator;


        private static Func<TIdDto, TIdDtoVM> fromDtoCreator;
        public static Func<TIdDto, TIdDtoVM> GetFromDtoCreator()
        {
            if (fromDtoCreator is null)
            {
                Type type = typeof(TIdDtoVM);
                ConstructorInfo ctor = type.GetConstructor(new Type[] { typeof(TIdDto) })
                    ?? throw new NotImplementedException($"В типе {type} не реализован конструктор принимающий тип {typeof(TIdDto)}.");
                fromDtoCreator = dto => (TIdDtoVM)ctor.Invoke(new object[] { dto });
            }
            return fromDtoCreator;
        }

        public IdDtoVMCollection()
            : this(GetFromDtoCreator())
        { }

        public IdDtoVMCollection(Func<TIdDto, TIdDtoVM> creator)
            : this(new SyncedObservableCollection<TIdDtoVM>())
        {
            this.creator = creator;
        }

        private IdDtoVMCollection(SyncedObservableCollection<TIdDtoVM> list)
            : base(list)
        {
            this.list = list;
        }

        public bool ReplaceOrAdd(TIdDto dto)
        {
            if (dict.TryGetValue(dto.Id, out TIdDtoVM vm))
            {
                vm.SetEntity(dto);
                return true;
            }
            else
            {
                vm = creator(dto);
                dict.Add(dto.Id, vm);
                list.Add(vm);
                return false;
            }
        }

        public bool Remove(TIdDto dto)
        {
            if (dict.TryGetValue(dto.Id, out TIdDtoVM vm))
            {
                dict.Remove(dto.Id);
                list.Remove(vm);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool ContainsId(int id)
        {
            return dict.ContainsKey(id);
        }
    }
}
