function debounceCountryFieldSelect(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};
function initVueMultiselectCountryRegion(element) {
    // only run script if element exists
    if (element) {
        var elementId = element.id;
        var selectedCountry = element.dataset.selectedCountry
        var selectedRegions = JSON.parse(element.dataset.selectedRegions || "[]");
        var loadCountryUrl = element.dataset.loadCountryUrl;
        var loadRegionUrl = element.dataset.loadRegionUrl;
        var multiple = JSON.parse(element.dataset.multiple);

        var debouncedCountriesLoad = debounceCountryFieldSelect(function (vm) {
            vm.isLoading = true;
            fetch(loadCountryUrl).then(function (res) {
                res.json().then(function (json) {
                    vm.countryOptions = json;
                    vm.isLoading = false;
                })
            });
        }, 250);

        var debouncedRegionsLoad = debounceCountryFieldSelect(function (vm, countryCode) {
            vm.isLoading = true;
            var loadFullUrl = loadRegionUrl;
            if (countryCode) {
                loadFullUrl += '/' + countryCode;
            }
            fetch(loadFullUrl).then(function (res) {
                res.json().then(function (json) {
                    vm.RegionOptions = json;
                    vm.isLoading = false;
                })
            });
        }, 250);

        var vueMultiselect = Vue.component('vue-multiselect', window.VueMultiselect.default);

        var vm = new Vue({
            el: '#' + elementId,
            components: { 'vue-multiselect': vueMultiselect },
            data: {
                countryValue: null,
                regionValue: null,
                arrayOfRegions: selectedRegions,
                country: selectedCountry,
                countryOptions: [],
                regionOptions: [],
            },
            computed: {
                selectedCountryCode: function () {
                    return this.countryValue.code;
                },
                selectedCountryName: function () {
                    return this.countryValue.name;
                },
                selectedRegionCodes: function () {
                    return this.regionValue.map(function (x) { return x.code }).join(',');
                },
                isDisabled: function () {
                    return this.arrayOfRegions.length > 0 && !multiple;
                }
            },
            watch: {
                selectedRegionCodes: function () {
                    // We add a delay to allow for the <input> to get the actual value	
                    // before the form is submitted	
                    setTimeout(function () { $(document).trigger('contentpreview:render') }, 100);
                }
            },
            created: function () {
                var self = this;
                self.loadCountries();
            },
            methods: {
                loadCountries: function () {
                    var self = this;
                    debouncedCountriesLoad(self);
                },
                loadRegions: function (countryCode) {
                    var self = this;
                    debouncedRegionsLoad(self, countryCode);
                },
                onSelectCountry: function (selectedOption) {
                    var self = this;
                    debouncedRegionsLoad(self, selectedOption.code);
                }
            }
        })
        
        /*Hook for other scripts that might want to have access to the view model*/
        var event = new CustomEvent("vue-multiselect-country-region", { detail: { vm: vm } });
        document.querySelector("body").dispatchEvent(event);
    }
}
