function loadUserStats(userid, containerId) {
            $.ajax({
                url: 'http://localhost:52856/api/users/GetUserStats?userId=' + userid,
                type: 'get',
                success: function(data) {
                    var html =
'<div class="w3-container">' +
    '<div class="w3-row-padding w3-margin-bottom w3-margin-top">'+
        '<div class="w3-quarter">'+
            '<div class="w3-container w3-red w3-padding-16">'+
                '<div class="w3-left"><i class="fa fa-credit-card w3-xxxlarge"></i></div>'+
            '   <div class="w3-right">'+
                    '<h3>' + data.OrdreTotal +'</h3>'+
                '</div>'+
                '<div class="w3-clear"></div>'+
                '<h4>Ordre</h4>'+
            '</div>'+
        '</div>'+
        '<div class="w3-quarter">'+
            '<div class="w3-container w3-blue w3-padding-16">'+
                '<div class="w3-left"><i class="fa fa-glass w3-xxxlarge"></i></div>'+
                '<div class="w3-right">'+
                    '<h3>' + data.Drinks +'</h3>'+
                '</div>'+
                '<div class="w3-clear"></div>'+
                '<h4>Drinks</h4>'+
            '</div>'+
        '</div>'+
        '<div class="w3-quarter">'+
            '<div class="w3-container w3-teal w3-padding-16">'+
                '<div class="w3-left"><i class="fa fa-vimeo w3-xxxlarge"></i></div>'+
                '<div class="w3-right">'+
                    '<h3>' + data.Varer +'</h3>'+
                '</div>'+
                '<div class="w3-clear"></div>'+
                '<h4>Varer</h4>'+
            '</div>'+
        '</div>'+
        '<div class="w3-quarter">'+
            '<div class="w3-container w3-orange w3-text-white w3-padding-16">'+
                '<div class="w3-left"><i class="fa fa-users w3-xxxlarge"></i></div>'+
                '<div class="w3-right">'+
                    '<h3>' + data.UserPlace +'</h3>'+
                '</div>'+
                '<div class="w3-clear"></div>'+
                '<h4>Top brugere</h4>'+
            '</div>'+
        '</div>'+
    '</div>'+
    '<div class="w3-row-padding w3-margin-bottom">' +
        '<div class="w3-col m12">' +
            '<canvas id="yearOrdreCount" style="display: block; width: 100%; height: 305px;"></canvas>' +
        '</div>' +
    '</div>' +
    '<div class="w3-row-padding w3-margin-bottom">' +
        '<div class="w3-col m12">' +
            '<canvas id="yearOrdrePrice" style="display: block; width: 100%; height: 305px;"></canvas>' +
        '</div>' +
    '</div>'+
'</div>';

                    $('#' + containerId).html(html);
                    var ordreCount = new Chart(document.getElementById("yearOrdreCount").getContext("2d"),
                        {
                            type: 'line',
                            data: {
                                labels: [
                                    'januar', 'februar', 'marts', 'april', 'maj', 'juni', 'juli', 'august', 'september',
                                    'oktober', 'november', 'december'
                                ],
                                datasets: [
                                    {
                                        label: "Total antal ordre",
                                        backgroundColor: "rgba(102, 204, 255,0.2)",
                                        borderColor: "rgba(102, 204, 255,1)",
                                        pointBorderColor: "#fff",
                                        data: data.OrdreTotalYear
                                    }
                                ]
                            },
                            options: {}
                        });
                    var ordrePrice = new Chart(document.getElementById("yearOrdrePrice").getContext("2d"),
                        {
                            type: 'line',
                            data: {
                                labels: [
                                    'januar', 'februar', 'marts', 'april', 'maj', 'juni', 'juli', 'august', 'september',
                                    'oktober', 'november', 'december'
                                ],
                                datasets: [
                                    {
                                        label: "Total beløb",
                                        backgroundColor: "rgba(102, 226, 40,0.2)",
                                        borderColor: "rgba(102, 226, 40,1)",
                                        pointBorderColor: "#fff",
                                        data: data.OrdrePriceTotalYear
                                    }
                                ]
                            },
                            options: {}
                        });
                }
            });
        }