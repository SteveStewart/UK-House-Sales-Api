// Find the form where name = formName and submit
function submitForm(formName) {

    $('#' + formName).submit();
};

// Set the input value on the dom element where the name = inputName 
function setInputValue(inputName, value) {
    
    $(':input[name="' + inputName + '"]').val(value);
};

function onTypeChanged(e) {

    var type = $(this).val();

    setInputValue(Resources.propertyTypeInputName, type);
    submitForm(Resources.filterFormInputName);
};

function onFilterChanged(e) {

    var period = $(this).val();

    setInputValue(Resources.searchPeriodInputName, period);
    submitForm(Resources.filterFormInputName);
};

function onOrderByAddressAscendingClicked(e) {

    setInputValue(Resources.orderByFieldInputName, Resources.addressOrderField);
    setInputValue(Resources.orderByDirectionInputName, Resources.orderByAscending);
    submitForm(Resources.filterFormInputName);
};

function onOrderByAddressDescendingClicked(e) {

    setInputValue(Resources.orderByFieldInputName, Resources.addressOrderField);
    setInputValue(Resources.orderByDirectionInputName, Resources.orderByDescending);
    submitForm(Resources.filterFormInputName);
};

function onOrderByLastSaleAscendingClicked(e) {

    setInputValue(Resources.orderByFieldInputName, Resources.lastSaleOrderField);
    setInputValue(Resources.orderByDirectionInputName, Resources.orderByAscending);
    submitForm(Resources.filterFormInputName);
};

function onOrderByLastSaleDescendingClicked(e) {

    setInputValue(Resources.orderByFieldInputName, Resources.lastSaleOrderField);
    setInputValue(Resources.orderByDirectionInputName, Resources.orderByDescending);
    submitForm(Resources.filterFormInputName);
};

function onOrderByPriceAscendingClicked(e) {

    setInputValue(Resources.orderByFieldInputName, Resources.pricePaidOrderField);
    setInputValue(Resources.orderByDirectionInputName, Resources.orderByAscending);
    submitForm(Resources.filterFormInputName);
};

function onOrderByPriceDescendingClicked(e) {

    setInputValue(Resources.orderByFieldInputName, Resources.pricePaidOrderField);
    setInputValue(Resources.orderByDirectionInputName, Resources.orderByDescending);
    submitForm(Resources.filterFormInputName);
};

function onPageLinkClicked(e) {
    
    var pageLinkId = $(this).attr('id');
    var id = pageLinkId.replace(Resources.pageLinkPrefix, '');

    setInputValue(Resources.pageInputName, id);

    submitForm(Resources.filterFormInputName);
};

function onSearchClicked(e) {
    
    setInputValue(Resources.pageInputName, "");
    setInputValue(Resources.propertyTypeInputName, "");
    setInputValue(Resources.searchPeriodInputName, "");
    setInputValue(Resources.orderByFieldInputName, "");
    setInputValue(Resources.orderByDirectionInputName, "");
};

$(document).ready(function () {

    $('#' + Resources.addressOrderAscInputName).click(onOrderByAddressAscendingClicked);
    $('#' + Resources.addressOrderDescInputName).click(onOrderByAddressDescendingClicked);
    $('#' + Resources.lastSaleOrderAscInputName).click(onOrderByLastSaleAscendingClicked);
    $('#' + Resources.lastSaleOrderDescInputName).click(onOrderByLastSaleDescendingClicked);
    $('#' + Resources.pricePaidOrderAscInputName).click(onOrderByPriceAscendingClicked);
    $('#' + Resources.pricePaidOrderDescInputName).click(onOrderByPriceDescendingClicked);

    $('span[id^="' + Resources.pageLinkPrefix +'"]').click(onPageLinkClicked);

    $('#' + Resources.searchButtonName).click(onSearchClicked);
    $('#' + Resources.typeDropdownName).on('changed.bs.select', onTypeChanged);
    $('#' + Resources.periodDropdownName).on('changed.bs.select', onFilterChanged);
});


