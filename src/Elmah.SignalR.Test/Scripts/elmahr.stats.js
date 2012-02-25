﻿Raphael.fn.pieChart = function (cx, cy, r, values, labels, stroke) {
    var paper = this,
        rad = Math.PI / 180,
        chart = this.set();
    function sector(cx, cy, r, startAngle, endAngle, params) {
        var x1 = cx + r * Math.cos(-startAngle * rad),
            x2 = cx + r * Math.cos(-endAngle * rad),
            y1 = cy + r * Math.sin(-startAngle * rad),
            y2 = cy + r * Math.sin(-endAngle * rad);
        return paper.path(["M", cx, cy, "L", x1, y1, "A", r, r, 0, +(endAngle - startAngle > 180), 0, x2, y2, "z"]).attr(params);
    }
    var angle = 0,
        start = 0,
        process = function (j, total) {
            var value = values[j],
                angleplus = 360 * value / total,
                popangle = angle + (angleplus / 2),
                color = Raphael.hsb(start, .75, 1),
                ms = 500,
                delta = -30,
                bcolor = Raphael.hsb(start, 1, 1),
                p = sector(cx, cy, r, angle, angle + angleplus, { fill: "90-" + bcolor + "-" + color, stroke: stroke, "stroke-width": 3 }),
                txt = paper.text(cx + (r + delta + 55) * Math.cos(-popangle * rad), cy + (r + delta + 25) * Math.sin(-popangle * rad), labels[j]).attr({ fill: "#222", stroke: "none", opacity: 0, "font-size": 12, "font-weight": "bold" });
            p.mouseover(function () {
                p.stop().animate({ transform: "s1.1 1.1 " + cx + " " + cy }, ms, "elastic");
                txt.stop().animate({ opacity: 1 }, ms, "elastic");
            }).mouseout(function () {
                p.stop().animate({ transform: "" }, ms, "elastic");
                txt.stop().animate({ opacity: 0 }, ms);
            });
            angle += angleplus;
            chart.push(p);
            chart.push(txt);
            start += .1;
        };
    var t = 0;
    for (var i = 0, ii = values.length; i < ii; i++) {
        var v = values[i];
        t += v;
    }
    var wasp = 0;
    for (i = 0; i < ii; i++) {
        process(i, t);
    }
    return chart;
};


elmahr.doStats = function (errors) {

    //errors count
    var total = _.chain(errors)
        .reduce(function (acc, e) { return acc + 1; }, 0)
        .value();
    $('#total').text(total);

    //error types count
    var types = _.chain(errors)
        .groupBy(function (e) { return e.shortType; })
        .reduce(function (acc, e) { return acc + 1; }, 0)
        .value();
    $('#types').text(types);

    //types stats
    elmahr.stats.removeAll();
    _.chain(errors)
        .groupBy(function (q) { return q.shortType; })
        .map(function (val, key) { return new KeyValuePair(key, val.length); })
        .sortBy(function (kvp) { return kvp.key; })
        .each(function (kvp) {
            if (kvp.key != undefined) 
                elmahr.stats.push(kvp);
        });

    //pie chart
    var values = [],
        labels = [];
    for (k in elmahr.stats()) {
        var kvp = elmahr.stats()[k];
        var key = kvp.key;
        if (key != undefined) {
            labels.push(key);
            values.push(kvp.value);
        }
    }

    if (values.length > 1) {
        $("#holder").html("");
        Raphael("holder", 300, 250).pieChart(150, 125, 80, values, labels, "#999");
    }

};