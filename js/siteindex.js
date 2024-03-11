
var urlsite = '/SiteInstruction/GetData'
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/siteHub").build();
var name;
var currentPage = 1; // Initialize the current page
var checkSearch = false;
var page = 1;
var pageSize = 10;
connection.start().then(function () {
    connection.on("LoadSiteData", function () {
        console.log("Successfully Connected");
        loadData(currentPage);
    });
}).catch(function (error) {
    loadData(currentPage);
    // This catch block will handle errors that occur during connection start.
    console.error("Connection start failed:", error);

});
$(window).on("popstate", function (e) {
    var state = e.originalEvent.state;
    if (state) {
        currentPage = state.page;
        loadData(currentPage);
    }
});

// ... existing code ...

loadData(currentPage);
function updatePagination(currentPage, totalPages) {
    var paginationHtml = '<ul class="pagination">';
    console.log(' paging currentPage = ' + currentPage);
    console.log('paging totalPages = ' + totalPages);
    // Add Previous links
    paginationHtml += '<li class="page-item ' + (currentPage <= 10 ? " disabled" : "") + '">';
    paginationHtml += '<a class="page-link" href="#" data-page="' + (currentPage - 10) + '">&lt;&lt; Previous</a>';
    paginationHtml += '</li>';

    paginationHtml += '<li class="page-item ' + (currentPage <= 1 ? " disabled" : "") + '">';
    paginationHtml += '<a class="page-link" href="#" data-page="' + (currentPage - 1) + '">Previous</a>';
    paginationHtml += '</li>';

    // Add current page
    paginationHtml += '<li class="page-item ' + (currentPage === totalPages ? "active" : "") + '">';
    paginationHtml += '<span class="page-link">' + currentPage + '</span>';
    paginationHtml += '</li>';

    // Add Next links
    paginationHtml += '<li class="page-item ' + (currentPage >= totalPages ? "disabled" : "") + '">';
    paginationHtml += '<a class="page-link" href="#" data-page="' + (currentPage + 1) + '">Next</a>';
    paginationHtml += '</li>';

    paginationHtml += '<li class="page-item ' + (currentPage + 9 >= totalPages ? "disabled" : "") + '">';
    paginationHtml += '<a class="page-link" href="#" data-page="' + (currentPage + 10) + '">Next >></a>';
    paginationHtml += '</li>';
    // Set the HTML
    $('#pagination').html(paginationHtml);

    // Handle click events on pagination links
    $('#pagination').on('click', 'a.page-link', function (e) {
        e.preventDefault();
        var newPage = parseInt($(this).data('page'));
        if (newPage !== currentPage) {
            currentPage = newPage;
            loadData(currentPage); // Call the loadData function when a page link is clicked
        }
    });
}
$(window).on("popstate", function (e) {
    // Handle the back/forward button navigation
    var state = e.originalEvent.state;
    if (state) {
        currentPage = state.page;
        loadData(currentPage);
    }
});
function loadData(page) {
    //alert("page" + page);
    $.ajax({
        url: urlsite,
        method: "GET",
        data: {
            page: page, // Pass the current page
            pageSize: pageSize,
        },
        success: function (result) {
            console.log(result)
            // Check if this is the initial search and no records were found
            if (checkSearch === true && result.data.length === 0) {
                // Display an alert and set checkSearch to false
                alert("No Records Found");
                checkSearch = false;
                // Clear the table body
                $('#tableBody').html('<tr><td colspan="7">No file match</td></tr>');

                // Reset the URL and reload data for the current page
                urlsite = "/SiteInstruction/GetData";
                loadData(currentPage);
            }
            else {
                // Data was found, proceed to populate the table
                var pl = '';
                $('#tableBody').empty();

                $.each(result.data, (k, v) => {
                    // Construct the table row HTML here
                    pl += `<tr>
                <td>${v.ProjectName}</td>
                <td>${v.CMPMINumber}</td>
                <td>${v.AsecPMINumber}</td>
                <td>${v.Status}</td>
                <td>${new Date(v.Date).toISOString().slice(0, 10)}</td>

    @if (User.IsInRole("Admin") || User.IsInRole("Cost") || User.IsInRole("Manager") || User.IsInRole("Engineering"))
    {
                <td>
                        <div id="CrudIcon" class="mt-2 fa-xs fa-sm fa-lg">
                             <a href='../SiteInstruction/Details?id=${v.CMSiteId}'>
                                                <i title="Details" class='far fa-file-alt'></i>
                                    </a>
                                    <a href='../SiteInstruction/Edit?id=${v.CMSiteId}'>
                                        <i title="Update" class='fa-sharp fa-solid fa-file-pen'></i>
                                    </a>
                @if (User.IsInRole("Admin"))
                {
                                            <a href='../SiteInstruction/Delete?id=${v.CMSiteId}' onclick="return doClick();">
                                                    <i title="Delete" class='icon fa-regular fa-trash-can'></i>
                                                </a>
                }
                </div>
                        </td>
    }
            </tr>`;
                });
                $('#tableBody').html(pl);

                updatePagination(result.pageCurrent, result.numSize);
            }
        },
        error: function (error) {
            alert("An error occurred while searching." + error);
        }
    });
}
$(document).on('click', 'a[href^="../SiteInstruction/Delete?id="]', function (e) {
    return window.confirm('Are you sure you want to delete this file?');
});

