
// Stores all earbuds retrieved from the API.
let allEarbuds = [];

// Get references to the HTML elements.
const searchInput = document.getElementById("searchInput");
const brandFilter = document.getElementById("brandFilter");
const minPrice = document.getElementById("minPrice");
const maxPrice = document.getElementById("maxPrice");
const featureFilters = document.getElementById("featureFilters");

const noiseCancellationFilter =
    document.getElementById("noiseCancellationFilter");

const transparencyFilter =
    document.getElementById("transparencyFilter");

const sortFilter = document.getElementById("sortFilter");
const clearFilters = document.getElementById("clearFilters");

const earbudResults = document.getElementById("earbudResults");
const resultCount = document.getElementById("resultCount");
const noResults = document.getElementById("noResults");
const loadingMessage = document.getElementById("loadingMessage");
const errorMessage = document.getElementById("errorMessage");

// Load earbuds when the page opens.
document.addEventListener("DOMContentLoaded", loadEarbuds);


// Retrieve earbuds from the API.
async function loadEarbuds() {
    try {
        const response = await fetch("/api/earbuds");

        if (!response.ok) {
            throw new Error("Unable to retrieve earbuds.");
        }

        allEarbuds = await response.json();

        loadingMessage.style.display = "none";

        // Populate the brand and feature filters.
        populateBrands();
        populateFeatures();

        // Display all earbuds initially.
        filterEarbuds();

    }
    catch (error) {
        loadingMessage.style.display = "none";
        errorMessage.textContent = error.message;
        errorMessage.style.display = "block";
    }
}


// Populate the brand dropdown.
function populateBrands() {
    const brands = [
        ...new Set(allEarbuds.map(earbud => earbud.brandName))
    ].sort();

    brands.forEach(brand => {
        const option = document.createElement("option");

        option.value = brand;
        option.textContent = brand;

        brandFilter.appendChild(option);
    });
}


// Populate feature checkboxes.
function populateFeatures() {
    const features = new Map();

    allEarbuds.forEach(earbud => {
        earbud.features.forEach(feature => {
            features.set(feature.featureId, feature.name);
        });
    });

    featureFilters.replaceChildren();

    [...features.entries()]
        .sort((a, b) => a[1].localeCompare(b[1]))
        .forEach(([id, name]) => {

            const wrapper = document.createElement("div");
            wrapper.className = "form-check";

            const checkbox = document.createElement("input");
            checkbox.type = "checkbox";
            checkbox.className = "form-check-input feature-filter";
            checkbox.value = id;
            checkbox.id = `feature-${id}`;

            const label = document.createElement("label");
            label.className = "form-check-label";
            label.htmlFor = checkbox.id;
            label.textContent = name;

            wrapper.append(checkbox, label);
            featureFilters.appendChild(wrapper);
        });
}


// Get the selected feature IDs.
function getSelectedFeatures() {
    return Array.from(
        document.querySelectorAll(".feature-filter:checked")
    ).map(checkbox => Number(checkbox.value));
}


// Apply all search filters.
function filterEarbuds() {

    const search = searchInput.value.trim().toLowerCase();
    const selectedBrand = brandFilter.value;

    const min = minPrice.value === ""
        ? null
        : Number(minPrice.value);

    const max = maxPrice.value === ""
        ? null
        : Number(maxPrice.value);

    const selectedFeatures = getSelectedFeatures();

    let filtered = allEarbuds.filter(earbud => {

        // Search by earbud name.
        const matchesName =
            earbud.name.toLowerCase().includes(search);

        // Filter by brand.
        const matchesBrand =
            selectedBrand === "" ||
            earbud.brandName === selectedBrand;

        // Filter by minimum price.
        const matchesMin =
            min === null || earbud.price >= min;

        // Filter by maximum price.
        const matchesMax =
            max === null || earbud.price <= max;

        // Filter by noise cancellation.
        const matchesNoise =
            !noiseCancellationFilter.checked ||
            earbud.noiseCancellation;

        // Filter by transparency mode.
        const matchesTransparency =
            !transparencyFilter.checked ||
            earbud.transparencyMode;

        // Every selected feature must exist on the earbud.
        const earbudFeatureIds = earbud.features.map(
            feature => feature.featureId
        );

        const matchesFeatures = selectedFeatures.every(
            id => earbudFeatureIds.includes(id)
        );

        return matchesName &&
            matchesBrand &&
            matchesMin &&
            matchesMax &&
            matchesNoise &&
            matchesTransparency &&
            matchesFeatures;
    });

    // Sort the filtered results.
    sortEarbuds(filtered);

    // Display the results.
    displayEarbuds(filtered);
}


// Sort the earbuds.
function sortEarbuds(earbuds) {

    const sortValue = sortFilter.value;

    switch (sortValue) {

        case "name-asc":
            earbuds.sort((a, b) =>
                a.name.localeCompare(b.name));
            break;

        case "name-desc":
            earbuds.sort((a, b) =>
                b.name.localeCompare(a.name));
            break;

        case "price-asc":
            earbuds.sort((a, b) => a.price - b.price);
            break;

        case "price-desc":
            earbuds.sort((a, b) => b.price - a.price);
            break;

        case "rating-desc":
            earbuds.sort((a, b) => b.rating - a.rating);
            break;

        case "battery-desc":
            earbuds.sort((a, b) =>
                b.batteryLife - a.batteryLife);
            break;
    }
}


// Display earbuds on the page.
function displayEarbuds(earbuds) {

    earbudResults.replaceChildren();

    resultCount.textContent =
        `Showing ${earbuds.length} of ${allEarbuds.length} earbuds`;

    noResults.style.display =
        earbuds.length === 0 ? "block" : "none";

    earbuds.forEach(earbud => {

        // Product column.
        const column = document.createElement("div");
        column.className = "col-md-6 col-xl-4";

        // Product card.
        const card = document.createElement("div");
        card.className = "card h-100 earbud-card";

        const body = document.createElement("div");
        body.className = "card-body d-flex flex-column";

        // Brand.
        const brand = document.createElement("p");
        brand.className = "text-secondary mb-1";
        brand.textContent = earbud.brandName;

        // Name.
        const name = document.createElement("h5");
        name.className = "card-title";
        name.textContent = earbud.name;

        // Price.
        const price = document.createElement("h4");
        price.className = "earbud-price";
        price.textContent =
            `$${Number(earbud.price).toFixed(2)}`;

        // Rating.
        const rating = document.createElement("p");
        rating.className = "mb-2";
        rating.textContent =
            `Rating: ${earbud.rating}/5`;

        // Specifications.
        const specifications = document.createElement("p");
        specifications.className = "small text-secondary";

        specifications.textContent =
            `${earbud.batteryLife} hours battery life`;

        // Features.
        const features = document.createElement("div");
        features.className = "mb-3";

        earbud.features.forEach(feature => {

            const badge = document.createElement("span");
            badge.className = "badge bg-secondary me-1 mb-1";
            badge.textContent = feature.name;

            features.appendChild(badge);
        });

        // Details button.
        const button = document.createElement("a");

        button.className = "btn btn-primary mt-auto";
        button.textContent = "View Details";
        button.href = `/Earbuds/Details/${earbud.earbudId}`;

        // Assemble the card.
        body.append(
            brand,
            name,
            price,
            rating,
            specifications,
            features,
            button
        );

        card.appendChild(body);
        column.appendChild(card);

        earbudResults.appendChild(column);
    });
}


// Listen for changes to the filters.
searchInput.addEventListener("input", filterEarbuds);
brandFilter.addEventListener("change", filterEarbuds);
minPrice.addEventListener("input", filterEarbuds);
maxPrice.addEventListener("input", filterEarbuds);

noiseCancellationFilter.addEventListener(
    "change", filterEarbuds
);

transparencyFilter.addEventListener(
    "change", filterEarbuds
);

sortFilter.addEventListener("change", filterEarbuds);

featureFilters.addEventListener("change", event => {
    if (event.target.matches(".feature-filter")) {
        filterEarbuds();
    }
});


// Reset all filters.
clearFilters.addEventListener("click", () => {

    searchInput.value = "";
    brandFilter.value = "";
    minPrice.value = "";
    maxPrice.value = "";

    noiseCancellationFilter.checked = false;
    transparencyFilter.checked = false;

    sortFilter.value = "name-asc";

    document.querySelectorAll(".feature-filter")
        .forEach(checkbox => {
            checkbox.checked = false;
        });

    filterEarbuds();
});
