﻿using Cofoundry.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cofoundry.Domain
{
    /// <summary>
    ///  This repository is used for looking up INestedDataModel types. An instance
    ///  of each is registered with the DI container which is intended to beused for looking
    ///  up definitions of model properties. This repository checks for duplicate type definitions
    ///  and will throw an exception on startup if duplicates are defined.
    /// </summary>
    public class NestedDataModelTypeRepository : INestedDataModelTypeRepository
    {
        const string MODEL_SUFFIX = "DataModel";

        private readonly Dictionary<string, INestedDataModel> _nestedDataModels;

        public NestedDataModelTypeRepository(
            IEnumerable<INestedDataModel> allNestedDataModels
            )
        {
            DetectDuplicates(allNestedDataModels);
            _nestedDataModels = allNestedDataModels.ToDictionary(m => GetDataModelName(m), StringComparer.OrdinalIgnoreCase);
        }

        private void DetectDuplicates(IEnumerable<INestedDataModel> allNestedDataModels)
        {
            var dulpicateCodes = allNestedDataModels
                .GroupBy(m => GetDataModelName(m))
                .Where(g => g.Count() > 1);

            if (dulpicateCodes.Any())
            {
                throw new Exception($"Duplicate INestedDataModel type name detected. {dulpicateCodes.First().Key}. Model names must be unique." );
            }
        }

        private string GetDataModelName(INestedDataModel model)
        {
            var typeName = model.GetType().Name;
            
            return StringHelper.RemoveSuffix(typeName, MODEL_SUFFIX, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets a specific INestedDataModel type by it's name. The
        /// name is the type name with the 'DataModel' suffix removed e.g. for the 
        /// data model type "CarouselItemDataModel", the name would be "CarouselItem".
        /// </summary>
        /// <param name="name">
        /// The name of the model to get. The
        /// name is the type name with the 'DataModel' suffix removed e.g. for the 
        /// data model type "CarouselItemDataModel", the name would be "CarouselItem".
        /// </param>
        public Type GetByName(string name)
        {
            var model = _nestedDataModels.GetOrDefault(name);
            return model?.GetType();
        }
    }
}
