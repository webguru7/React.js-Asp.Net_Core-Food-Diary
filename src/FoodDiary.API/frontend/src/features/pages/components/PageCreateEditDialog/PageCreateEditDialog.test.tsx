import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { create } from 'src/test-utils';
import { PageCreateEdit } from '../../models';
import PageCreateEditDialog from './PageCreateEditDialog';

test('page can be created', async () => {
  const submitFn = jest.fn();
  const ui = create
    .component(
      <PageCreateEditDialog
        open={true}
        onDialogConfirm={submitFn}
        onDialogCancel={jest.fn()}
      ></PageCreateEditDialog>,
    )
    .withReduxStore()
    .withMuiPickersUtils()
    .please();

  render(ui);
  await waitFor(() => expect(screen.getByLabelText(/page date/i)).toHaveDisplayValue('05.06.2022'));
  await userEvent.click(screen.getByText(/create/i));

  expect(submitFn).toHaveBeenCalledWith({
    date: '2022-06-05',
  } as PageCreateEdit);
});
