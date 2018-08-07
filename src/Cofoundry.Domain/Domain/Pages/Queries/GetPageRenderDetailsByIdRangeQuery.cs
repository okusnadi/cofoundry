﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cofoundry.Domain.CQS;
using Cofoundry.Core.Validation;

namespace Cofoundry.Domain
{
    /// <summary>
    /// Gets a range of pages by their PageIds as PageRenderDetails objects. A PageRenderDetails contains 
    /// the data required to render a page, including template data for all the content-editable regions.
    /// </summary>
    public class GetPageRenderDetailsByIdRangeQuery : IQuery<IDictionary<int, PageRenderDetails>>
    {
        public GetPageRenderDetailsByIdRangeQuery() { }

        /// <summary>
        /// Initializes the query with the specified parameters.
        /// </summary>
        /// <param name="pageIds">PageIds of the pages to get.</param>
        /// <param name="publishStatus">Used to determine which version of the page to include data for.</param>
        public GetPageRenderDetailsByIdRangeQuery(IEnumerable<int> pageIds, PublishStatusQuery? publishStatus = null)
            : this(pageIds?.ToList(), publishStatus)
        {
        }

        /// <summary>
        /// Initializes the query with the specified parameters.
        /// </summary>
        /// <param name="pageIds">PageIds of the pages to get.</param>
        /// <param name="publishStatus">Used to determine which version of the page to include data for.</param>
        public GetPageRenderDetailsByIdRangeQuery(IReadOnlyCollection<int> pageIds, PublishStatusQuery? publishStatus = null)
        {
            PageIds = pageIds;
            if (publishStatus.HasValue)
            {
                PublishStatus = publishStatus.Value;
            }
        }

        /// <summary>
        /// Database ids of the pages to get.
        /// </summary>
        public IReadOnlyCollection<int> PageIds { get; set; }

        /// <summary>
        /// Used to determine which version of the page to include data for. This 
        /// defaults to Latest, meaning that a draft page will be returned ahead of a
        /// published version of the page.
        /// </summary>
        public PublishStatusQuery PublishStatus { get; set; }
    }
}
