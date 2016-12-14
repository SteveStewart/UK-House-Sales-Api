using HouseSales.Domain;
using HouseSales.Repositories;
using HouseSales.Web.Models;
using System;
using System.Web.Mvc;

namespace HouseSales.Web.Infrastructure.ModelBinders
{
    public class PropertyListFormSubmitModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            PropertyListFormSubmitModel model = 
                bindingContext.Model as PropertyListFormSubmitModel ?? new PropertyListFormSubmitModel();

            model.Postcode = GetPostcode(bindingContext, Properties.Resources.TXT_POSTCODE_INPUT_NAME);
            model.Page = GetPage(bindingContext, Properties.Resources.TXT_PAGE_INPUT_NAME);
            model.PropertyType = GetPropertyType(bindingContext, Properties.Resources.TXT_PROPERTY_TYPE_INPUT_NAME);
            model.SearchRangeInMonths = GetSearchRangeInMonths(bindingContext, Properties.Resources.TXT_SEARCH_PERIOD_INPUT_NAME);

            OrderField orderField = GetOrderField(bindingContext, Properties.Resources.TXT_ORDER_BY_FIELD_INPUT_NAME);
            OrderDirection orderDirection = GetOrderDirection(bindingContext, Properties.Resources.TXT_ORDER_BY_DIRECTION_INPUT_NAME);

            model.OrderBy = new OrderBy(orderField, orderDirection);

            return model;
        }

        private OrderDirection GetOrderDirection(ModelBindingContext context, string name)
        {
            ValueProviderResult result = context.ValueProvider.GetValue(name);
            
            if (result == null)
                return OrderDirection.Ascending;

            return ParseOrderDirection(result.AttemptedValue);
        }

        private OrderField GetOrderField(ModelBindingContext context, string name)
        {
            ValueProviderResult result = context.ValueProvider.GetValue(name);

            if (result == null)
                return OrderField.Address;

            return ParseOrderField(result.AttemptedValue);
        }

        private int GetSearchRangeInMonths(ModelBindingContext context, string name)
        {
            ValueProviderResult result = context.ValueProvider.GetValue(name);

            if (result == null)
                return 0;

            return ParsePeriod(result.AttemptedValue);
        }

        private PropertyType GetPropertyType(ModelBindingContext context, String name)
        {
            ValueProviderResult result = context.ValueProvider.GetValue(name);

            if (result == null)
                return PropertyType.None;

            return ParsePropertyType(result.AttemptedValue);
        }

        private int GetPage(ModelBindingContext context, String name)
        {
            int value = 0;

            ValueProviderResult result = context.ValueProvider.GetValue(name);

            if (result == null)
                return 0;

            int.TryParse(result.AttemptedValue, out value);
                
            return value;
        }

        private String GetPostcode(ModelBindingContext context, String name)
        {
            ValueProviderResult result = context.ValueProvider.GetValue(name);

            if (result == null)
                return String.Empty;

            return result.AttemptedValue.ToUpper();                        
        }

        /// <summary>
        /// Parse a PropertyType enum 
        /// </summary>
        /// <param name="typeString"></param>
        /// <returns></returns>
        private PropertyType ParsePropertyType(String value)
        {
            PropertyType propertyType = PropertyType.None;

            if (Enum.TryParse<PropertyType>(value, out propertyType))
                return propertyType;

            return PropertyType.None;
        }

        /// <summary>
        /// Parse the search range in months
        /// </summary>
        /// <param name="periodString"></param>
        /// <returns></returns>
        private int ParsePeriod(String periodString)
        {
            int periodValue = 0;

            if (periodString == String.Empty)
                return 0;

            if (int.TryParse(periodString, out periodValue))
                return periodValue;

            return Int32.MaxValue;
        }

        /// <summary>
        /// Parse the order by field
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private OrderField ParseOrderField(String value)
        {
            OrderField orderBy = OrderField.None;

            if (Enum.TryParse<OrderField>(value, out orderBy))
                return orderBy;

            return OrderField.None;
        }

        /// <summary>
        /// Parse the order by direction
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private OrderDirection ParseOrderDirection(String value)
        {
            OrderDirection direction = OrderDirection.Ascending;

            if (Enum.TryParse<OrderDirection>(value, out direction))
                return direction;

            return OrderDirection.Ascending;
        }

    }
}