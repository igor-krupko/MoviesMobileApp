using System;
using System.Collections.Generic;
using System.Linq;
using MoviesMobileApp.Common;
using MoviesMobileApp.Mappers;

namespace MoviesMobileApp.Utils
{
    public static class ObservableCollectionHelper
    {
        public static ExtendedObservableCollection<TModel> CreateFrom<TModel>(object source, Action<TModel> modelPrepareAction = null)
        {
            var models = MappingConfigurator.Mapper.Map<IEnumerable<TModel>>(source).ToList();
            models.ForEach(modelPrepareAction ?? delegate { });
            return new ExtendedObservableCollection<TModel>(models);
        }

        public static void UpdateFrom<TModel, TDto>(this ExtendedObservableCollection<TModel> collection, IEnumerable<TDto> source, Action<TModel> modelPrepareAction = null)
            where TModel : IIdentifiable
            where TDto : IIdentifiable
        {
            using (var delayed = collection.DelayNotifications())
            {
                foreach (var dto in source)
                {
                    var model = collection.FirstOrDefault(x => Equals(x.Identity, dto.Identity));

                    if (model != null)
                    {
                        MappingConfigurator.Mapper.Map(dto, model);
                    }
                    else
                    {
                        var newModel = MappingConfigurator.Mapper.Map<TModel>(dto);
                        modelPrepareAction?.Invoke(newModel);
                        delayed.Add(newModel);
                    }
                }
            }
        }
    }
}