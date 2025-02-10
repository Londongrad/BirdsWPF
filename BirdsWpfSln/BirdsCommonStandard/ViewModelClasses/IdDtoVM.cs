using System;

namespace BirdsCommonStandard
{
    /// <summary>Оболочка с уведомлением для сущностей Модели с идентификатором.</summary>
    /// <typeparam name="TIdDto">Тип сущности Модели с обязательным Id.</typeparam>
    public class IdDtoVM<TIdDto> : DtoVM<TIdDto>
        where TIdDto : IdDto
    {
        /// <summary>Id добавляемых сущностей.</summary>
        public int Id { get; }

        public IdDtoVM(int id)
        {
            Id = id;
        }

        public IdDtoVM(TIdDto dto)
            : this(dto.Id)
        {
            SetEntity(dto);
        }

        /// <summary>В метод <see cref="DtoVM{TDto}.SetEntity(TDto)"/> добавлена проверка Id сущности.</summary>
        /// <param name="entity">Добавляемая сущность или <see langword="null"/>.</param>
        /// <exception cref="ArgumentException">Если Id сущности не совпадает с <see cref="Id"/>.</exception>
        public override void SetEntity(TIdDto entity)
        {
            if (!(entity is null) && Id != entity.Id)
                throw new ArgumentException("entity.Id должен совпадать с Id.");
            base.SetEntity(entity);
        }
    }
}
