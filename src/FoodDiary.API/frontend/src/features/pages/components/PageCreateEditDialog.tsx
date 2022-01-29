import 'date-fns';
import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  DialogProps,
} from '@material-ui/core';
import { MuiPickersUtilsProvider, KeyboardDatePicker } from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import dateFnsFormat from 'date-fns/format';
import { PageCreateEdit } from '../models';
import { DialogCustomActionProps } from '../../__shared__/types';
import { getDateForNewPage } from '../thunks';
import { useTypedSelector, useValidatedDateInput } from '../../__shared__/hooks';
import { createDateValidator } from '../../__shared__/validators';

interface PageCreateEditDialogProps extends DialogProps, DialogCustomActionProps<PageCreateEdit> {
  page?: PageCreateEdit;
}

const PageCreateEditDialog: React.FC<PageCreateEditDialogProps> = ({
  page,
  onDialogConfirm,
  onDialogCancel,
  ...dialogProps
}: PageCreateEditDialogProps) => {
  const dateForNewPage = useTypedSelector(state =>
    state.pages.dateForNewPage ? new Date(state.pages.dateForNewPage) : null,
  );

  const dateForNewPageLoading = useTypedSelector(state => state.pages.dateForNewPageLoading);

  const dispatch = useDispatch();

  const { title, submitText, initialDate } = page
    ? {
        title: 'Edit page',
        submitText: 'Save',
        initialDate: new Date(page.date),
      }
    : {
        title: 'New page',
        submitText: 'Create',
        initialDate: new Date(),
      };

  const [date, setDate, bindDate, isValidDate] = useValidatedDateInput(initialDate, {
    validate: createDateValidator(true),
    errorHelperText: 'Date is required',
  });

  const isSubmitDisabled = !isValidDate;

  useEffect(() => {
    if (dialogProps.open) {
      setDate(initialDate);

      if (!page) {
        dispatch(getDateForNewPage());
      }
    }
  }, [dialogProps.open]);

  useEffect(() => {
    if (dateForNewPageLoading === 'succeeded' && !page) {
      setDate(dateForNewPage);
    }
  }, [dateForNewPageLoading]);

  const handleSubmitClick = (): void => {
    if (!date) {
      return;
    }

    onDialogConfirm({
      date: dateFnsFormat(date, 'yyyy-MM-dd'),
    });
  };

  return (
    <Dialog maxWidth="xs" fullWidth {...dialogProps}>
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <MuiPickersUtilsProvider utils={DateFnsUtils}>
          <KeyboardDatePicker
            {...bindDate()}
            label="Page date"
            placeholder="dd.MM.yyyy"
            format="dd.MM.yyyy"
            margin="normal"
            fullWidth
            autoFocus
          />
        </MuiPickersUtilsProvider>
      </DialogContent>
      <DialogActions>
        <Button
          variant="contained"
          color="primary"
          onClick={handleSubmitClick}
          disabled={isSubmitDisabled}
        >
          {submitText}
        </Button>
        <Button variant="text" onClick={onDialogCancel}>
          Cancel
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default PageCreateEditDialog;
