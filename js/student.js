function stuShow(d, g, c) {
            var urlat = "../teacher/studentshow.aspx?sid=" + d + "&sgrade=" + g + "&sclass=" + c;
            openLessonModal(urlat, "学生详情", 700);
        }
        function stuAdd(g, c) {
            var urlad = "../teacher/studentadd.aspx?sgrade=" + g + "&sclass=" + c;
            openLessonModal(urlad, "添加学生", 700);
        }
