(() => {
    var xValues = ["2013", "2014", "2015", "2016", "2017","2018", "2019", "2020", "2022", "2023"];
    var yValues = [5500, 99958, 4433, 2454, 1500, 256, 45821, 2368, 23677,7852];
    var barColors = ["green", "green", "green", "green", "green", "green", "green", "green", "green", "green",];

    new Chart("myChart", {
        type: "bar",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]
        },
        options: {
            legend: { display: false },
            responsive: true,
            maintainAspectRatio: false,
            title: {
                display: true,
                text: "Grafico Geral CDO"
            }
        }
    });
})()
    