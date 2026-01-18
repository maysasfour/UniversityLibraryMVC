document.addEventListener('DOMContentLoaded', function () {

    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            let isValid = true;

            const requiredFields = this.querySelectorAll('[required]');
            requiredFields.forEach(field => {
                if (!field.value.trim()) {
                    isValid = false;
                    const errorElement = field.nextElementSibling;
                    if (errorElement && errorElement.classList.contains('text-danger')) {
                        errorElement.style.display = 'block';
                    }
                    field.classList.add('is-invalid');
                }
            });

            if (!isValid) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        });
    });

    const searchInputs = document.querySelectorAll('.form-control[placeholder*="Search"]');
    searchInputs.forEach(input => {
        input.addEventListener('keyup', function () {
            const searchTerm = this.value.toLowerCase();
            const tableRows = this.closest('.card-body')?.querySelectorAll('table tbody tr');

            if (tableRows) {
                tableRows.forEach(row => {
                    const text = row.textContent.toLowerCase();
                    row.style.display = text.includes(searchTerm) ? '' : 'none';
                });
            }
        });
    });

    const tabLinks = document.querySelectorAll('.nav-link[data-bs-toggle="tab"]');
    tabLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const target = this.getAttribute('href');
            document.querySelectorAll('.tab-pane').forEach(pane => {
                pane.classList.remove('show', 'active');
            });
            document.querySelector(target).classList.add('show', 'active');
        });
    });

    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.classList.add('fade');
            setTimeout(() => {
                alert.style.display = 'none';
            }, 150);
        }, 5000);
    });
});