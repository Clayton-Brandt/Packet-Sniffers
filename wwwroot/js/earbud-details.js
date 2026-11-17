
document.addEventListener("DOMContentLoaded", loadEarbudDetails);


async function loadEarbudDetails() {

    const loadingMessage =
        document.getElementById("loadingMessage");

    const errorMessage =
        document.getElementById("errorMessage");

    const detailsContainer =
        document.getElementById("earbudDetails");

    try {

        // Get the earbud ID from the current URL.
        const pathParts = window.location.pathname.split("/");
        const earbudId = pathParts[pathParts.length - 1];

        // Make sure the ID is valid.
        if (!/^\d+$/.test(earbudId) || Number(earbudId) < 1) {
            throw new Error("Invalid earbud ID.");
        }

        // Retrieve the earbud from the API.
        const response = await fetch(
            `/api/earbuds/${encodeURIComponent(earbudId)}`
        );

        if (!response.ok) {

            if (response.status === 404) {
                throw new Error("Earbud not found.");
            }

            throw new Error("Unable to retrieve earbud details.");
        }

        const earbud = await response.json();

        // Populate the page.
        document.getElementById("brandName").textContent =
            earbud.brandName;

        document.getElementById("earbudName").textContent =
            earbud.name;

        document.getElementById("earbudPrice").textContent =
            `$${Number(earbud.price).toFixed(2)}`;

        document.getElementById("earbudRating").textContent =
            `Rating: ${earbud.rating}/5`;

        document.getElementById("batteryLife").textContent =
            `${earbud.batteryLife} hours`;

        document.getElementById("noiseCancellation").textContent =
            earbud.noiseCancellation ? "Yes" : "No";

        document.getElementById("transparencyMode").textContent =
            earbud.transparencyMode ? "Yes" : "No";

        document.getElementById("waterResistance").textContent =
            earbud.waterResistance || "Not specified";

        document.getElementById("bluetoothVersion").textContent =
            earbud.bluetoothVersion || "Not specified";

        // Display associated features.
        const featuresContainer =
            document.getElementById("earbudFeatures");

        featuresContainer.replaceChildren();

        if (earbud.features.length === 0) {

            const message = document.createElement("p");
            message.className = "text-secondary";
            message.textContent = "No additional features listed.";

            featuresContainer.appendChild(message);

        } else {

            earbud.features.forEach(feature => {

                const featureCard =
                    document.createElement("div");

                featureCard.className =
                    "card p-3 mb-2 w-100";

                const featureName =
                    document.createElement("h6");

                featureName.textContent = feature.name;

                const featureDescription =
                    document.createElement("p");

                featureDescription.className =
                    "text-secondary mb-0";

                featureDescription.textContent =
                    feature.description || "";

                featureCard.append(
                    featureName,
                    featureDescription
                );

                featuresContainer.appendChild(featureCard);
            });
        }

        // Show the completed page.
        loadingMessage.style.display = "none";
        detailsContainer.style.display = "block";

    }
    catch (error) {

        loadingMessage.style.display = "none";

        errorMessage.textContent = error.message;
        errorMessage.style.display = "block";
    }
}
