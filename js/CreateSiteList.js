$(document).ready(function () {
    let urlsite = '/SiteInstruction/GetData';
    let currentPage = 1;
    let pageSize = 10;

    loadData(currentPage);
    function updatePagination(currentPage, totalPages) {
        var paginationHtml = '<ul class="pagination">';
        console.log(' paging currentPage = ' + currentPage);
        console.log('paging totalPages = ' + totalPages);
        // Add Previous links
        paginationHtml += '<li class="page-item ' + (currentPage <= 10 ? "disabled" : "") + '">';
        paginationHtml += '<a class="page-link" href="#" data-page="' + (currentPage - 10) + '">&lt;&lt; Previous</a>';
        paginationHtml += '</li>';

        paginationHtml += '<li class="page-item ' + (currentPage <= 1 ? "disabled" : "") + '">';
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

    function loadData(page) {
        $.ajax({
            url: urlsite,
            method: "GET",
            data: {
                page: page,
                pageSize: pageSize,
            },
            success: function (result) {
                console.log(result);
                if (result.data.length === 0) {
                    console.log("No Records Found");
                    checkSearch = false;
                    $('#tableBody').html('<tr><td colspan="7">No file match</td></tr>');
                    loadData(currentPage);
                } else {
                    // Data was found, proceed to populate the table
                    let tableHtml = '';

                    $.each(result.data, (k, v) => {
                        console.log("data");
                        // Construct the table row HTML here
                        tableHtml += `<tr>
                                <td>${v.ProjectName}</td>
                                <td>${v.CMPMINumber}</td>
                                <td>${v.AsecPMINumber}</td>
                                <td>${v.Status}</td>
                                <td>${new Date(v.Date).toISOString().slice(0, 10)}</td>
                                
                            </tr>`;
                    });

                    $('#tableBody').html(tableHtml);

                    console.log("Connected");
                    currentPage = result.pageCurrent;
                    totalPages = result.numSize;
                    updatePagination(currentPage, totalPages);
                }
            },
            error: function (error) {
                console.error("An error occurred while searching.", error);
            }
        });
    }

});