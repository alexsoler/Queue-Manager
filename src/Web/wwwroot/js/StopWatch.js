﻿
class StopWatch {
    constructor(display) {
        this.running = false;
        this.display = display;
        this.reset();
        this.print(this.time);
    }

    reset() {
        this.times = [0, 0, 0];
    }

    start() {
        if (!this.time) this.time = performance.now();
        if (!this.running) {
            this.running = true;

            requestAnimationFrame(this.step.bind(this));
        }
    }

    stop() {
        this.running = false;
        this.time = null;
    }

    restart() {
        if (!this.time) this.time = performance.now();
        if (!this.running) {
            this.running = true;

            requestAnimationFrame(this.step.bind(this));
        }

        this.reset();
    }

    step(timestamp) {
        if (!this.running) return;
        this.calculate(timestamp);
        this.time = timestamp;
        this.print();
        requestAnimationFrame(this.step.bind(this));
    }

    calculate(timestamp) {
        var diff = timestamp - this.time;

        this.times[2] += diff / 10;

        if (this.times[2] >= 100) {
            this.times[1] += 1;
            this.times[2] -= 100;
        }

        if (this.times[1] >= 60) {
            this.times[0] += 1;
            this.times[1] -= 60;
        }
    }

    print() {
        this.display.innerText = this.format(this.times);
    }

    format(times) {
        return `\
                ${pad0(times[0], 2)}:\
                ${pad0(times[1], 2)}`;
    }
}

function pad0(value, count) {
    var result = value.toString();
    for (; result.length < count; --count)
        result = '0' + result;
    return result;
}

function clearChildren(node) {
    while (node.lastChild)
        node.removeChild(node.lastChild);
}

let stopwatch = new StopWatch(
    document.querySelector('#stopwatch'));
