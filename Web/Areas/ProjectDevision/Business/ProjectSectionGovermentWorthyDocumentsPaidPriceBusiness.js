var ProjectSectionGovermentWorthyDocumentsPaidPricePermission;


function IndexProjectSectionGovermentWorthyDocumentsPaidPriceCallBack(Id)
{
    alert(Id);
    ProjectSectionGovermentWorthyDocumentsPaidPricePermission = $('#permission-ProjectSectionGovermentWorthyDocumentsPaidPrice').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionGovermentWorthyDocumentsPaidPrice/_Create", ProjectSectionGovermentWorthyDocumentsPaidPricePermission) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionGovermentWorthyDocumentsPaidPrice/_Create/' + Id, '', '#FormContainer-ProjectSectionGovermentWorthyDocumentsPaidPrice', 'CreateProjectSectionGovermentWorthyDocumentsPaidPriceCallBack(' + Id + ')');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionGovermentWorthyDocumentsPaidPrice/_List", ProjectSectionGovermentWorthyDocumentsPaidPricePermission) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionGovermentWorthyDocumentsPaidPrice/_List', '', '#FormList-ProjectSectionGovermentWorthyDocumentsPaidPrice', 'ListProjectSectionGovermentWorthyDocumentsPaidPriceCallBack(' + Id + ')');
    }
}


function CreateProjectSectionGovermentWorthyDocumentsPaidPriceCallBack(Id)
{
    alert('create');
}


function ListProjectSectionGovermentWorthyDocumentsPaidPriceCallBack(Id)
{
    alert('list');
}