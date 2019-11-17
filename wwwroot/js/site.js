// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function active(id) {
  $("#" + id).addClass("active");
}

function loadStudentsList(tableName, page) {
  id = $(".tab-slider--trigger.active").prop("id");
  sort = null;
  order = null;
  tab = $("#" + id);

  if ($(".fa-fw.fa-sort-up").length == 1) {
    sort = $(".fa-fw.fa-sort-up")
      .attr("class")
      .split(" ")[0];
    order = "asc";
  } else if ($(".fa-fw.fa-sort-down").length == 1) {
    sort = $(".fa-fw.fa-sort-down")
      .attr("class")
      .split(" ")[0];
    order = "desc";
  }

  keyword = $("#searchStudents").val();
  limit = parseInt($("#" + tableName + "-limit").val());
  if (id == "tabCard") {
    limit += 1;
  }
  offset = page == 1 ? 0 : (page - 1) * limit;

  let studentIds = JSON.parse(localStorage.getItem("studentIds"));

  $.ajax({
    type: "Post",
    url: "Home/LoadStudent",
    data: {
      tableName: tableName,
      keyword: keyword,
      limit: limit,
      offset: offset || 0,
      studentIds: studentIds,
      sort: sort,
      order: order
    },
    dataType: "json",
    error: function(resp) {
      $("#" + tableName).html(resp.responseText);
      countTotal(tableName, keyword, limit, studentIds, sort, order, page || 1);
      setIcon(sort, order);
      changeTab(tab);
      document.querySelectorAll('input[type="checkbox"]').forEach(checkbox => {
        if (
          studentIds.includes(parseInt(checkbox.getAttribute("student-id")))
        ) {
          checkbox.checked = true;
        }
      });
    }
  });
}

function countTotal(tableName, keyword, limit, studentIds, sort, order, page) {
  $.ajax({
    type: "Post",
    url: "Home/LoadStudent",
    data: {
      tableName: tableName,
      keyword: keyword,
      limit: limit,
      offset: 0,
      studentIds: studentIds,
      sort: sort,
      order: order,
      count: true
    },
    dataType: "json",
    success: function(resp) {
      paginationStudents(tableName, page, resp, limit);
    }
  });
}

function paginationStudents(tableName, pageSelected, total, limit) {
  page = "";
  total = Math.ceil(total / limit);
  for (var i = 0; i <= total - 1; i++) {
    page = i + 1;
    offset = page == 1 ? 0 : (page - 1) * limit;
    active = pageSelected == page ? "active" : "";
    $("#" + tableName + "-paginationStudents").append(
      '<li class="page-item ' +
        active +
        '" id="' +
        page +
        '" ><a class="page-link cicle" onclick="loadStudentsList("' +
        tableName +
        '", ' +
        page +
        ')" href="javascript:void(null)">' +
        page +
        "</a></li>"
    );
  }
}

function removeStudent(id) {
  var confirmation = confirm("are you sure you want to remove the item?");
  if (confirmation) {
    $.ajax({
      type: "Delete",
      url: "Home/Delete/",
      data: { id: id },
      dataType: "json",
      success: function(resp) {
        if (resp) {
          loadStudentsList(
            "StudentsTable",
            parseInt($(".page-item.active").prop("id"))
          );
        }
      }
    });
  }
}

function sortStudent(sort) {
  page = parseInt($(".page-item.active").prop("id"));
  icon = $("." + sort);
  if (icon.hasClass("fa-sort")) {
    resetIcon();
    icon.removeClass("fa-sort");
    icon.addClass("fa-sort-up");
    icon.addClass("text-success");
    loadStudentsList("StudentsTable", page);
  } else if (icon.hasClass("fa-sort-up")) {
    resetIcon();
    icon.removeClass("fa-sort-up");
    icon.addClass("fa-sort-down");
    icon.addClass("text-success");
    loadStudentsList("StudentsTable", page);
  } else {
    resetIcon();
    loadStudentsList("StudentsTable", page);
  }
}

function resetIcon() {
  moreIcon = $(".fa-fw");
  icon.removeClass("text-success");
  moreIcon.removeClass("fa-sort-up");
  moreIcon.removeClass("fa-sort-down");
  moreIcon.addClass("fa-sort");
}

function setIcon(sort, order) {
  icon = $("." + sort);
  if (order == "asc") {
    icon.removeClass("fa-sort");
    icon.addClass("fa-sort-up");
    icon.addClass("text-success");
  } else if (order == "desc") {
    icon.removeClass("fa-sort-up");
    icon.addClass("fa-sort-down");
    icon.addClass("text-success");
  } else {
    resetIcon();
  }
}

function changeTab(self) {
  $(".tab-slider--body").hide();
  var activeTab = $(self).attr("rel");
  $("#" + activeTab).fadeIn();
  if ($(self).attr("rel") == "tab2") {
    $(".tab-slider--tabs").addClass("slide");
  } else {
    $(".tab-slider--tabs").removeClass("slide");
  }
  $(".tab-slider--nav li").removeClass("active");
  $(self).addClass("active");
}

function checkedAll(id) {
  let input = $("#" + id);
  document.querySelectorAll('input[type="checkbox"]').forEach(checkbox => {
    checkbox.checked = input.is(":checked");
  });
}

function getStudentsById() {
  let studentIds = [];
  document
    .querySelectorAll('input[type="checkbox"]:checked')
    .forEach(checkbox =>
      studentIds.push(parseInt(checkbox.getAttribute("student-id")))
    );

  localStorage.setItem("studentIds", JSON.stringify(studentIds));
}
