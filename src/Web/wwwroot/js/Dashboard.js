Chart.defaults.global.pointHitDetectionRadius = 1;
Chart.defaults.global.tooltips.enabled = false;
Chart.defaults.global.tooltips.mode = 'index';
Chart.defaults.global.tooltips.position = 'nearest';
Chart.defaults.global.tooltips.custom = CustomTooltips; // eslint-disable-next-line no-unused-vars

var cardChart1 = new Chart($('#chartUsers'), {
    type: 'line',
    data: {
        labels: months,
        datasets: [{
            label: 'Usuarios agregados',
            backgroundColor: getStyle('--primary'),
            borderColor: 'rgba(255,255,255,.55)',
            data: dataChartUsers
        }]
    },
    options: {
        maintainAspectRatio: false,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                gridLines: {
                    color: 'transparent',
                    zeroLineColor: 'transparent'
                },
                ticks: {
                    fontSize: 2,
                    fontColor: 'transparent'
                }
            }],
            yAxes: [{
                display: false,
                ticks: {
                    display: false,
                    min: 0,
                    max: 10
                }
            }]
        },
        elements: {
            line: {
                borderWidth: 1
            },
            point: {
                radius: 4,
                hitRadius: 10,
                hoverRadius: 4
            }
        }
    }
}); // eslint-disable-next-line no-unused-vars

var cardChart2 = new Chart($('#chartOffices'), {
    type: 'line',
    data: {
        labels: months,
        datasets: [{
            label: 'Oficinas agregadas',
            backgroundColor: getStyle('--info'),
            borderColor: 'rgba(255,255,255,.55)',
            data: dataChartOffices
        }]
    },
    options: {
        maintainAspectRatio: false,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                gridLines: {
                    color: 'transparent',
                    zeroLineColor: 'transparent'
                },
                ticks: {
                    fontSize: 2,
                    fontColor: 'transparent'
                }
            }],
            yAxes: [{
                display: false,
                ticks: {
                    display: false,
                    min: 0,
                    max: 10
                }
            }]
        },
        elements: {
            line: {
                tension: 0.00001,
                borderWidth: 1
            },
            point: {
                radius: 4,
                hitRadius: 10,
                hoverRadius: 4
            }
        }
    }
}); // eslint-disable-next-line no-unused-vars

var cardChart3 = new Chart($('#chartMedias'), {
    type: 'line',
    data: {
        labels: months,
        datasets: [{
            label: 'Multimedia agregada',
            backgroundColor: 'rgba(255,255,255,.2)',
            borderColor: 'rgba(255,255,255,.55)',
            data: dataChartMedias
        }]
    },
    options: {
        maintainAspectRatio: false,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                display: false
            }],
            yAxes: [{
                display: false
            }]
        },
        elements: {
            line: {
                borderWidth: 2
            },
            point: {
                radius: 0,
                hitRadius: 10,
                hoverRadius: 4
            }
        }
    }
}); // eslint-disable-next-line no-unused-vars

var cardChart4 = new Chart($('#chartComments'), {
    type: 'bar',
    data: {
        labels: months,
        datasets: [{
            label: 'Comentarios emitidos',
            backgroundColor: 'rgba(255,255,255,.2)',
            borderColor: 'rgba(255,255,255,.55)',
            data: dataChartComments
        }]
    },
    options: {
        maintainAspectRatio: false,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                display: false,
                barPercentage: 0.6
            }],
            yAxes: [{
                display: false
            }]
        }
    }
}); // eslint-disable-next-line no-unused-vars

var random = function random() {
    return Math.round(Math.random() * 50);
};

var lineChart = new Chart($('#chartTickets'), {
    type: 'line',
    data: {
        labels: months,
        datasets: [{
            label: 'Clientes atendidos',
            backgroundColor: 'rgba(220, 220, 220, 0.2)',
            borderColor: 'rgba(220, 220, 220, 1)',
            pointBackgroundColor: 'rgba(220, 220, 220, 1)',
            pointBorderColor: '#fff',
            data: dataTicketsProcesed
        }, {
            label: 'Clientes no atendidos',
            backgroundColor: 'rgba(151, 187, 205, 0.2)',
            borderColor: 'rgba(151, 187, 205, 1)',
            pointBackgroundColor: 'rgba(151, 187, 205, 1)',
            pointBorderColor: '#fff',
            data: dataTicketsNotProcesed
        }]
    },
    options: {
        responsive: true
    }
}); // eslint-disable-next-line no-unused-vars

