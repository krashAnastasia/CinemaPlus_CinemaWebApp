document.addEventListener('DOMContentLoaded', () => {
  const heroSlider = document.querySelector('[data-hero-slider]');

  if (heroSlider) {
    const slides = Array.from(heroSlider.querySelectorAll('[data-hero-slide]'));
    const dots = Array.from(heroSlider.querySelectorAll('[data-hero-dot]'));
    const reduceMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const interval = Number(heroSlider.getAttribute('data-hero-interval') || '5000');
    const initialIndex = Math.max(0, slides.findIndex((slide) => slide.classList.contains('is-active')));
    let activeIndex = initialIndex === -1 ? 0 : initialIndex;
    let timerId = null;

    const renderSlide = (index, enableExit = true) => {
      slides.forEach((slide, slideIndex) => {
        const isActive = slideIndex === index;
        const wasActive = slideIndex === activeIndex;

        slide.classList.toggle('is-active', isActive);
        slide.classList.toggle('is-exiting', enableExit && wasActive && !isActive);
      });

      dots.forEach((dot, dotIndex) => {
        const isActive = dotIndex === index;
        dot.classList.toggle('is-active', isActive);
        dot.setAttribute('aria-current', isActive ? 'true' : 'false');
      });

      activeIndex = index;
    };

    const advanceSlide = () => {
      if (slides.length < 2) {
        return;
      }

      const nextIndex = (activeIndex + 1) % slides.length;
      renderSlide(nextIndex);
    };

    const stopAutoPlay = () => {
      if (timerId !== null) {
        window.clearInterval(timerId);
        timerId = null;
      }
    };

    const startAutoPlay = () => {
      if (reduceMotion || slides.length < 2 || timerId !== null) {
        return;
      }

      timerId = window.setInterval(advanceSlide, interval);
    };

    dots.forEach((dot, index) => {
      dot.addEventListener('click', () => {
        renderSlide(index);
        stopAutoPlay();
        startAutoPlay();
      });
    });

    heroSlider.addEventListener('mouseenter', stopAutoPlay);
    heroSlider.addEventListener('mouseleave', startAutoPlay);
    heroSlider.addEventListener('focusin', stopAutoPlay);
    heroSlider.addEventListener('focusout', () => {
      if (!heroSlider.contains(document.activeElement)) {
        startAutoPlay();
      }
    });

    slides.forEach((slide) => {
      slide.classList.remove('is-active', 'is-exiting');
    });

    dots.forEach((dot) => {
      dot.classList.remove('is-active');
      dot.setAttribute('aria-current', 'false');
    });

    if (reduceMotion) {
      renderSlide(activeIndex, false);
      startAutoPlay();
    } else {
      window.requestAnimationFrame(() => {
        window.requestAnimationFrame(() => {
          renderSlide(activeIndex, false);
          startAutoPlay();
        });
      });
    }
  }

  const profileRoot = document.querySelector('[data-profile-root]');

  if (profileRoot) {
    const displayCard = profileRoot.querySelector('[data-profile-display]');
    const editForm = profileRoot.querySelector('[data-profile-form]');
    const openButtons = profileRoot.querySelectorAll('[data-profile-edit-open]');
    const cancelButton = profileRoot.querySelector('[data-profile-edit-cancel]');
    const photoForm = profileRoot.querySelector('[data-profile-photo-form]');
    const photoInput = profileRoot.querySelector('[data-profile-photo-input]');

    const setProfileEditMode = (isEditing) => {
      displayCard?.classList.toggle('is-hidden', isEditing);
      editForm?.classList.toggle('is-visible', isEditing);

      if (isEditing) {
        const firstInput = editForm?.querySelector('input');
        firstInput?.focus();
      }
    };

    openButtons.forEach((button) => {
      button.addEventListener('click', () => {
        setProfileEditMode(true);
      });
    });

    cancelButton?.addEventListener('click', () => {
      setProfileEditMode(false);
    });

    photoInput?.addEventListener('change', () => {
      if (photoInput.files && photoInput.files.length > 0) {
        photoForm?.requestSubmit();
      }
    });
  }

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
