document.addEventListener('DOMContentLoaded', function () {

    const isbnInput = document.getElementById('ISBN');
    if (isbnInput) {
        isbnInput.addEventListener('blur', function () {
            const isbn = this.value.trim();
            const regex = /^(?:ISBN(?:-1[03])?:? )?(?=[-0-9 ]{17}$|[-0-9X ]{13}$|[0-9X]{10}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?(?:[0-9]+[- ]?){2}[0-9X]$/;

            const isValid = regex.test(isbn);
            if (!isValid && isbn) {
                this.classList.add('is-invalid');
                const errorElement = document.querySelector(`[data-valmsg-for="ISBN"]`);
                if (errorElement) {
                    errorElement.innerHTML = 'Invalid ISBN format. Use format like 978-3-16-148410-0';
                }
            } else {
                this.classList.remove('is-invalid');
                const errorElement = document.querySelector(`[data-valmsg-for="ISBN"]`);
                if (errorElement) {
                    errorElement.innerHTML = '';
                }
            }
        });
    }

    const yearInput = document.getElementById('PublicationYear');
    if (yearInput) {
        yearInput.addEventListener('blur', function () {
            const year = parseInt(this.value);
            const currentYear = new Date().getFullYear();

            if (year && (year < 1800 || year > currentYear)) {
                this.classList.add('is-invalid');
                const errorElement = document.querySelector(`[data-valmsg-for="PublicationYear"]`);
                if (errorElement) {
                    errorElement.innerHTML = `Year must be between 1800 and ${currentYear}`;
                }
            } else {
                this.classList.remove('is-invalid');
                const errorElement = document.querySelector(`[data-valmsg-for="PublicationYear"]`);
                if (errorElement) {
                    errorElement.innerHTML = '';
                }
            }
        });
    }

    const searchInputs = document.querySelectorAll('.form-control[placeholder*="Search"]');
    searchInputs.forEach(input => {
        input.addEventListener('keyup', function () {
            const searchTerm = this.value.toLowerCase();
            const tableRows = this.closest('.card-body').querySelectorAll('table tbody tr');

            tableRows.forEach(row => {
                const text = row.textContent.toLowerCase();
                row.style.display = text.includes(searchTerm) ? '' : 'none';
            });
        });
    });

    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            let isValid = true;

            const requiredFields = form.querySelectorAll('[required], .form-control');
            requiredFields.forEach(field => {
                if (field.hasAttribute('required') && !field.value.trim()) {
                    isValid = false;
                    field.classList.add('is-invalid');
                    const errorElement = field.nextElementSibling;
                    if (errorElement && errorElement.classList.contains('text-danger')) {
                        errorElement.style.display = 'block';
                    }
                }
            });

            if (!isValid) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        });
    });
});