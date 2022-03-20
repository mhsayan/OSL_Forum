using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BO = OSL.Forum.Core.BusinessObjects;
using EO = OSL.Forum.Core.Entities;
using OSL.Forum.Core.UnitOfWorks;

namespace OSL.Forum.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICoreUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CategoryService(ICoreUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BO.Category GetCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentNullException(nameof(categoryName));

            var categoryEntity = _unitOfWork.Categories.Get(c => c.Name == categoryName, "").FirstOrDefault();

            if (categoryEntity == null)
                return null;

            var category = _mapper.Map<BO.Category>(categoryEntity);

            return category;
        }

        public void CreateCategory(BO.Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            var oldCategory = GetCategory(category.Name);

            if (oldCategory != null)
                throw new DuplicateNameException("This category name already exists.");

            category.CreationDate = DateTime.Now;
            category.ModificationDate = category.CreationDate;

            var categoryEntity = _mapper.Map<EO.Category>(category);

            _unitOfWork.Categories.Add(categoryEntity);
            _unitOfWork.Save();
        }
    }
}
