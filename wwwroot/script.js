document.addEventListener('DOMContentLoaded', () => {
  const burger = document.getElementById('navbarBurger');
  const mobileMenu = document.getElementById('mobileMenu');

  if (burger && mobileMenu) {
    const setAria = (open) => {
      burger.setAttribute('aria-expanded', open ? 'true' : 'false');
    };

    burger.addEventListener('click', () => {
      burger.classList.toggle('active');
      mobileMenu.classList.toggle('active');

      const isOpen = burger.classList.contains('active');
      setAria(isOpen);
    });

    const mobileLinks = mobileMenu.querySelectorAll('a');
    mobileLinks.forEach((link) => {
      link.addEventListener('click', () => {
        burger.classList.remove('active');
        mobileMenu.classList.remove('active');
        setAria(false);
      });
    });
  }

  // Chip selection logic
  const chipGroups = document.querySelectorAll('[data-group]');

  chipGroups.forEach(group => {
    const chips = group.querySelectorAll('.chip');

    chips.forEach(chip => {
      chip.addEventListener('click', () => {
        chips.forEach(c => c.classList.remove('chip--active'));
        chip.classList.add('chip--active');
      });
    });
  });

  // Seat selection logic
  const seatElements = document.querySelectorAll('.seat');
  const seatSelectionRoot = document.querySelector('[data-seat-selection-root]');

  if (seatSelectionRoot) {
    const selectedSeatsNode = seatSelectionRoot.querySelector('[data-selected-seats]');
    const selectedCountNode = seatSelectionRoot.querySelector('[data-selected-count]');
    const selectedTotalNode = seatSelectionRoot.querySelector('[data-selected-total]');
    const selectedSeatInputsNode = seatSelectionRoot.querySelector('[data-selected-seat-inputs]');
    const orderButton = seatSelectionRoot.querySelector('[data-order-button]');
    const seatPrice = Number(seatSelectionRoot.getAttribute('data-seat-price') || '0');

    const renderSeatSelectionSummary = () => {
      const selectedSeats = Array.from(
        seatSelectionRoot.querySelectorAll('.seat--selected[data-seat-id]')
      );

      if (selectedSeatsNode) {
        selectedSeatsNode.textContent = selectedSeats.length > 0
          ? selectedSeats.map(seat => seat.getAttribute('data-seat-label')?.toUpperCase()).join(' • ')
          : 'МІСЦЯ ЩЕ НЕ ОБРАНІ';
      }

      if (selectedTotalNode) {
        selectedTotalNode.textContent = `ДО СПЛАТИ: ${(seatPrice * selectedSeats.length).toFixed(2)} грн`;
      }

      if (selectedSeatInputsNode) {
        selectedSeatInputsNode.innerHTML = '';

        selectedSeats.forEach(seat => {
          const input = document.createElement('input');
          input.type = 'hidden';
          input.name = 'SelectedSeatIds';
          input.value = seat.getAttribute('data-seat-id') || '';
          selectedSeatInputsNode.appendChild(input);
        });
      }

      if (orderButton) {
        orderButton.disabled = selectedSeats.length === 0;
      }
    };

    seatElements.forEach(seat => {
      if (seat.classList.contains('seat--unavailable')) return;

      seat.addEventListener('click', () => {
        if (seat.classList.contains('seat--selected')) {
          seat.classList.remove('seat--selected');
          seat.classList.add('seat--available');
        } else if (seat.classList.contains('seat--available')) {
          seat.classList.remove('seat--available');
          seat.classList.add('seat--selected');
        }

        renderSeatSelectionSummary();
      });
    });

    renderSeatSelectionSummary();
  } else {
    seatElements.forEach(seat => {
      if (seat.classList.contains('seat--unavailable')) return;

      seat.addEventListener('click', () => {
        if (seat.classList.contains('seat--selected')) {
          seat.classList.remove('seat--selected');
          seat.classList.add('seat--available');
        } else if (seat.classList.contains('seat--available')) {
          seat.classList.remove('seat--available');
          seat.classList.add('seat--selected');
        }
      });
    });
  }

  // Checkout payment form formatting and numeric validation
  const cardInput = document.getElementById('card-number');
  if (cardInput) {
    cardInput.addEventListener('input', (e) => {
      let value = e.target.value.replace(/\D/g, '').substring(0, 16);
      value = value.replace(/(.{4})/g, '$1 ').trim();
      e.target.value = value;
    });
  }

  const expInput = document.getElementById('card-exp');
  if (expInput) {
    expInput.addEventListener('input', (e) => {
      let value = e.target.value.replace(/\D/g, '').substring(0, 4);

      if (value.length >= 3) {
        value = value.substring(0, 2) + '/' + value.substring(2);
      }

      e.target.value = value;
    });
  }

  const cvcInput = document.getElementById('card-cvc');
  if (cvcInput) {
    cvcInput.addEventListener('input', (e) => {
      e.target.value = e.target.value.replace(/\D/g, '').substring(0, 3);
    });
  }

  const checkoutProfileToggle = document.querySelector('[data-checkout-profile-toggle]');
  const checkoutProfileDisplay = document.querySelector('[data-checkout-profile-display]');
  const checkoutProfileEditor = document.querySelector('[data-checkout-profile-editor]');
  const checkoutProfileState = document.querySelector('[data-checkout-profile-state]');

  if (checkoutProfileToggle && checkoutProfileDisplay && checkoutProfileEditor && checkoutProfileState) {
    const setProfileEditorState = (isOpen) => {
      checkoutProfileDisplay.hidden = isOpen;
      checkoutProfileEditor.hidden = !isOpen;
      checkoutProfileToggle.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
      checkoutProfileState.value = isOpen ? 'true' : 'false';

      if (isOpen) {
        const firstInput = checkoutProfileEditor.querySelector('input');
        if (firstInput) {
          firstInput.focus();
        }
      }
    };

    setProfileEditorState(checkoutProfileState.value === 'true');

    checkoutProfileToggle.addEventListener('click', () => {
      const isOpen = checkoutProfileToggle.getAttribute('aria-expanded') === 'true';
      setProfileEditorState(!isOpen);
    });
  }

  const trailerVideo = document.querySelector('[data-trailer-video]');
  const trailerPlayButton = document.querySelector('[data-trailer-play]');
  const trailerOverlay = document.querySelector('[data-trailer-overlay]');

  if (trailerVideo && trailerPlayButton && trailerOverlay) {
    const hideTrailerChrome = () => {
      trailerPlayButton.classList.add('is-hidden');
      trailerOverlay.classList.add('is-hidden');
    };

    const showTrailerChrome = () => {
      trailerPlayButton.classList.remove('is-hidden');
      trailerOverlay.classList.remove('is-hidden');
    };

    trailerPlayButton.addEventListener('click', async () => {
      trailerVideo.controls = true;

      try {
        await trailerVideo.play();
        hideTrailerChrome();
      } catch {
        showTrailerChrome();
      }
    });

    trailerVideo.addEventListener('play', hideTrailerChrome);
    trailerVideo.addEventListener('pause', showTrailerChrome);
    trailerVideo.addEventListener('ended', showTrailerChrome);
  }
});
